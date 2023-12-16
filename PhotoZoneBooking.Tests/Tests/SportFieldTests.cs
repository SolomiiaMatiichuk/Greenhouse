using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.BLL.Interfaces;
using PhotoZoneBooking.BLL.Profiles;
using PhotoZoneBooking.BLL.Services;
using PhotoZoneBooking.DAL;
using PhotoZoneBooking.DAL.Entities;
using PhotoZoneBooking.DAL.Repositories;
using Xunit;

namespace PhotoZoneBooking.Tests.Tests;

public class PhotoZoneTests : IDisposable
{
    private IPhotoZoneService _photoZoneService;
    private readonly IMapper _mapper;
    private DataContext _context;

    private void InitTempInstances()
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        var dbName = Guid.NewGuid().ToString();
        builder.UseInMemoryDatabase(dbName);
        _context = new DataContext(builder.Options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        var photoZoneRepository = new GenericRepository<PhotoZone>(_context);
        var detailRepository = new GenericRepository<PhotoZoneDetail>(_context);
        _photoZoneService = new PhotoZoneService(photoZoneRepository, detailRepository, _mapper);
    }

    private async Task InitTempPhotoZone()
    {
        await _context.PhotoZones.AddAsync(new PhotoZone
        {
            Id = 1,
            ImageUrl = "Test",
            PricePerHour = 999,
            Title = "Test"
        });
        await _context.SaveChangesAsync();
        await _context.PhotoZoneDetails.AddAsync(new PhotoZoneDetail
        {
            Address = "Test",
            Description = "Test",
            EndProgram = "22-00",
            StartProgram = "11-00",
            PhotoZoneId = 1
        });
        await _context.SaveChangesAsync();
    }
    
    public PhotoZoneTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MapperProfile());
        });
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task CreatePhotoZone_Test()
    {
        //arrange
        InitTempInstances();
        var model = new PhotoZoneDto
        {
            Id = 1,
            ImageUrl = "Test",
            Address = "Test",
            Description = "Test",
            EndProgram = "22-00",
            PricePerHour = 999,
            StartProgram = "11-00",
            Title = "Test"
        };
        
        //act
        await _photoZoneService.CreatePhotoZone(model);
        
        var result = await _context.PhotoZones.SingleOrDefaultAsync(t => t.Id == model.Id);
        
        //assert
        Assert.Equal(model.Id, result.Id);
    }

    [Fact]
    public async Task GetPhotoZone_Test()
    {
        //arrange
        InitTempInstances();
        await InitTempPhotoZone();
        
        //act
        var result = await _photoZoneService.GetPhotoZoneWithDetailsAsync(1);
        
        //assert
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task UpdatePhotoZone_Test()
    {
        //arrange
        InitTempInstances();
        await InitTempPhotoZone();
        var modelToUpdate = new PhotoZoneDto
        {
            Id = 1,
            PhotoZoneDetailId = 1,
            ImageUrl = "UpdatedTest",
            Address = "UpdatedTest",
            Description = "UpdatedTest",
            EndProgram = "22-00",
            PricePerHour = 999,
            StartProgram = "11-00",
            Title = "UpdatedTest"
        };
        
        //act
        await _photoZoneService.UpdatePhotoZone(modelToUpdate);
        
        //assert
        var result = await _context.PhotoZones.SingleOrDefaultAsync(t => t.Id == modelToUpdate.Id);
        Assert.Equal(modelToUpdate.Title, result.Title);
    }

    [Fact]
    public async Task DeletePhotoZone_Test()
    {
        InitTempInstances();
        await InitTempPhotoZone();
        var dataBeforeDelete = await _context.PhotoZones.ToListAsync();
        var countBeforeDelete = dataBeforeDelete.Count;
        await _photoZoneService.DeletePhotoZone(1);
        var dataAfterDelete = await _context.PhotoZones.ToListAsync();
        var countAfterDelete = dataAfterDelete.Count;
        Assert.Equal(countBeforeDelete - 1, countAfterDelete);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}