using System.Net;
using AutoMapper;
using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.BLL.Interfaces;
using PhotoZoneBooking.DAL.Entities;
using PhotoZoneBooking.DAL.Interfaces;

namespace PhotoZoneBooking.BLL.Services;

public class PhotoZoneService : IPhotoZoneService
{
    private readonly IGenericRepository<PhotoZone> _repository;
    private readonly IGenericRepository<PhotoZoneDetail> _detailRepository;
    private readonly IMapper _mapper;

    public PhotoZoneService(IGenericRepository<PhotoZone> repository, IGenericRepository<PhotoZoneDetail> detailRepository, IMapper mapper)
    {
        _repository = repository;
        _detailRepository = detailRepository;
        _mapper = mapper;
    }

    public async Task<PhotoZoneDto> GetPhotoZoneWithDetailsAsync(int photoZoneId)
    {
        var field = await _repository.GetByIdAsync(photoZoneId);
        return _mapper.Map<PhotoZone, PhotoZoneDto>(field);
    }

    public async Task<List<PhotoZoneDto>> GetPhotoZonesWithDetailsAsync()
    {
        var fields = await _repository.GetAllAsync();
        return _mapper.Map<List<PhotoZone>, List<PhotoZoneDto>>(fields);
    }

    public async Task<PhotoZoneCallback> CreatePhotoZone(PhotoZoneDto model)
    {
        var startScheduleHours = Convert.ToDouble(model.StartProgram.Split('-')[0]);
        var startScheduleMinutes = Convert.ToDouble(model.StartProgram.Split('-')[1]);
        
        var endScheduleHours = Convert.ToDouble(model.EndProgram.Split('-')[0]);
        var endScheduleMinutes = Convert.ToDouble(model.EndProgram.Split('-')[1]);

        if (startScheduleHours is < 0 or > 24 || 
            endScheduleHours is < 0 or > 24 || 
            startScheduleMinutes is < 0 or > 59 || 
            endScheduleMinutes is < 0 or > 59)
        {
            return new PhotoZoneCallback
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = "Enter valid time"
            };
        }
        
        var field = new PhotoZone
        {
            Title = model.Title,
            PricePerHour = model.PricePerHour,
            ImageUrl = model.ImageUrl
        };
        await _repository.InsertAsync(field);

        var detail = new PhotoZoneDetail
        {
            Description = model.Description,
            Address = model.Address,
            EndProgram = model.EndProgram,
            StartProgram = model.StartProgram,
            PhotoZoneId = field.Id
        };
        await _detailRepository.InsertAsync(detail);

        return new PhotoZoneCallback
        {
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task DeletePhotoZone(int id)
    {
        var field = await _repository.GetByIdAsync(id);
        _repository.DetachLocal(field, id);
        await _repository.DeleteAsync(field);
    }

    public async Task UpdatePhotoZone(PhotoZoneDto model)
    {
        var updatedField = new PhotoZone
        {
            Id = model.Id,
            Title = model.Title,
            PricePerHour = model.PricePerHour,
            ImageUrl = model.ImageUrl
        };
        await _repository.UpdateAsync(updatedField);
        
        var updatedDetail = new PhotoZoneDetail
        {
            Id = model.PhotoZoneDetailId,
            Description = model.Description,
            Address = model.Address,
            EndProgram = model.EndProgram,
            StartProgram = model.StartProgram,
            PhotoZoneId = model.Id
        };
        await _detailRepository.UpdateAsync(updatedDetail);
    }
}