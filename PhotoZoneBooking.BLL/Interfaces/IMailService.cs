using PhotoZoneBooking.BLL.Dtos;

namespace PhotoZoneBooking.BLL.Interfaces;

public interface IMailService
{
    Task SendMailAsync(string to, string subject, string message);
    Task SendInvoiceAsync(string to, ReservationDto model);
    Task SendReservationCancelMailAsync(string to, int reservationId);
}