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

public class ReservationTests : IDisposable
{
    private IReservationService _reservationService;
    private DataContext _context;
    private readonly IMapper _mapper;

    private void InitTempInstances()
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        var dbName = Guid.NewGuid().ToString();
        builder.UseInMemoryDatabase(dbName);
        _context = new DataContext(builder.Options);
        var reservationRepository = new GenericRepository<Reservation>(_context);
        var photoZoneRepository = new GenericRepository<PhotoZone>(_context);
        _reservationService = new ReservationService(reservationRepository, _mapper, photoZoneRepository);
    }

    private async Task InitTempPhotoZone()
    {
        await _context.PhotoZones.AddAsync(new PhotoZone
        {
            Id = 1,
            Title = "Test",
            ImageUrl = "Test",
            PricePerHour = 999
        });
        await _context.SaveChangesAsync();
        await _context.PhotoZoneDetails.AddAsync(new PhotoZoneDetail
        {
            Address = "Test",
            PhotoZoneId = 1,
            Description = "Test",
            StartProgram = "10-00",
            EndProgram = "23-00"
        });
        await _context.SaveChangesAsync();
    }

    private async Task InitTempUser()
    {
        await _context.Users.AddAsync(new User
        {
            Id = "UserTestId",
            UserName = "TestUser",
            Email = "Test@email.com",
            FirstName = "TestUser",
            LastName = "TestUser",
            PhoneNumber = "Test",
            PasswordHash = "UserTestHash"
        });
        await _context.SaveChangesAsync();
    }
    
    public ReservationTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MapperProfile());
        });
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task CreateReservation_Test()
    {
        //arrange
        InitTempInstances();
        await InitTempPhotoZone();
        await InitTempUser();

        var model = new ReservationDto
        {
            Id = 1,
            Start = new DateTime(2022, 7, 10, 12, 0, 0),
            End = new DateTime(2022, 7, 10, 18, 0, 0),
            UserId = "UserTestId",
            PhotoZoneId = 1,
            Status = "Pending",
            Title = "Test",
        };

        await _reservationService.CreateReservationAsync(model);

        //act
        var result = await _context.Reservations.SingleOrDefaultAsync(t => t.Id == model.Id);
        
        //assert
        Assert.Equal(model.Title, result.Title);
    }

    [Fact]
    public async Task GetReservation_Test()
    {
        //arrange
        InitTempInstances();
        await InitTempPhotoZone();
        await InitTempUser();
        _context.Reservations.Add(new Reservation
        {
            Id = 77,
            Start = new DateTime(1974, 7, 10, 7, 10, 24),
            End = new DateTime(1974, 7, 10, 10, 0, 24),
            UserId = "UserTestId",
            PhotoZoneId = 1,
            Title = "Test",
        });
        await _context.SaveChangesAsync();
        
        //act
        var result = await _reservationService.GetReservationAsync(77);
        
        //assert
        Assert.Equal("Test", result.Title);
    }
    [Fact]
    public async Task DeleteReservation_Test()
    {
        InitTempInstances();
        await InitTempPhotoZone();
        await InitTempUser();
        
        _context.Reservations.Add(new Reservation
        {
            Id = 77,
            Start = new DateTime(1974, 7, 10, 7, 10, 24),
            End = new DateTime(1974, 7, 10, 10, 0, 24),
            UserId = "UserTestId",
            PhotoZoneId = 1,
            Title = "Test",
        });
        await _context.SaveChangesAsync();
        var dataBeforeDelete = await _context.Reservations.ToListAsync();
        var countBeforeDelete = dataBeforeDelete.Count;
        await _reservationService.DeleteReservationAsync(77);
        var dataAfterDelete = await _context.Reservations.ToListAsync();
        var countAfterDelete = dataAfterDelete.Count;
        Assert.Equal(countBeforeDelete - 1, countAfterDelete);
    }

    [Fact]
    public async Task UpdateReservation_Test()
    {
        InitTempInstances();
        await InitTempUser();
        await InitTempPhotoZone();
        
        _context.Reservations.Add(new Reservation
        {
            Id = 77,
            Start = new DateTime(1974, 7, 10, 7, 10, 24),
            End = new DateTime(1974, 7, 10, 10, 0, 24),
            UserId = "UserTestId",
            PhotoZoneId = 1,
            Title = "Test",
        });
        await _context.SaveChangesAsync();

        var modelToUpdate = new ReservationDto
        {
            Id = 77,
            Start = new DateTime(1974, 7, 10, 7, 10, 24),
            End = new DateTime(1974, 7, 10, 10, 0, 24),
            UserId = "UserTestId",
            PhotoZoneId = 1,
            Title = "Updated",
        };

        await _reservationService.UpdateReservationAsync(modelToUpdate);

        var result = await _context.Reservations.SingleOrDefaultAsync(t => t.Id == modelToUpdate.Id);
        Assert.Equal(modelToUpdate.Title, result.Title);
    }

    [Fact]
    public async Task GetReservationsByTitle_Test()
    {
        //arrange
        InitTempInstances();
        await InitTempUser();
        await InitTempPhotoZone();
        
        await _context.Reservations.AddAsync(new Reservation
        {
            Id = 77,
            Start = new DateTime(1974, 7, 10, 7, 10, 24),
            End = new DateTime(1974, 7, 10, 10, 0, 24),
            UserId = "UserTestId",
            PhotoZoneId = 1,
            Title = "Test",
        });
        await _context.SaveChangesAsync();
        
        //act
        var result = await _reservationService.GetPhotoZoneReservationsByTitleAsync("Test", 1);
        
        //assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.First().Title);
    }

    [Fact]
    public async Task CancelReservationTest()
    {
        //arrange
        InitTempInstances();
        await InitTempUser();
        await InitTempPhotoZone();
        
        await _context.Reservations.AddAsync(new Reservation
        {
            Id = 77,
            Start = new DateTime(1974, 7, 10, 7, 10, 24),
            End = new DateTime(1974, 7, 10, 10, 0, 24),
            UserId = "UserTestId",
            PhotoZoneId = 1,
            Title = "Test",
        });
        await _context.SaveChangesAsync();
        
        //act
        await _reservationService.CancelReservationAsync(77);
        
        //assert
        var result = await _context.Reservations.SingleOrDefaultAsync(t => t.Id == 77);
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}