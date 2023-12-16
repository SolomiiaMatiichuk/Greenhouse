using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.DAL.Entities;

namespace PhotoZoneBooking.BLL.Interfaces;

public interface IReservationService
{
    Task<ReservationDto> PayReservationAsync(int id);
    Task<ReservationCallback> CreateReservationAsync(ReservationDto reservation);
    Task<ReservationCallback> UpdateReservationAsync(ReservationDto reservation);
    Task DeleteReservationAsync(int reservationId);
    Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(string userId);
    Task<IEnumerable<ReservationDto>> GetPhotoZoneReservationsAsync(int photoZoneId);
    Task<IEnumerable<ReservationDto>> GetPhotoZoneReservationsByTitleAsync(string title, int photoZoneId);
    Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
    Task<ReservationDto> GetReservationAsync(int id);
    Task CancelReservationAsync(int reservationId);
}