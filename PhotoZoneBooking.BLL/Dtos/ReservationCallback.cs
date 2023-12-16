using System.Net;

namespace PhotoZoneBooking.BLL.Dtos;

public class ReservationCallback
{
    public HttpStatusCode StatusCode { get; set; }
    public string Error { get; set; }
}