using System.Net;

namespace PhotoZoneBooking.BLL.Dtos;

public class PhotoZoneCallback
{
    public HttpStatusCode StatusCode { get; set; }
    public string Error { get; set; }
}