using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.DAL.Entities;

namespace PhotoZoneBooking.BLL.Interfaces;

public interface IPhotoZoneService
{
    Task<PhotoZoneDto> GetPhotoZoneWithDetailsAsync(int photoZoneId);
    Task<List<PhotoZoneDto>> GetPhotoZonesWithDetailsAsync();
    Task<PhotoZoneCallback> CreatePhotoZone(PhotoZoneDto model);
    Task DeletePhotoZone(int id);
    Task UpdatePhotoZone(PhotoZoneDto model);
}