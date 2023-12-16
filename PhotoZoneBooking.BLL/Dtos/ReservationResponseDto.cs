namespace PhotoZoneBooking.BLL.Dtos;

public class ReservationResponseDto
{
    public int Id { get; set; }
    public string PhotoZoneTitle { get; set; }
    public string ReservationTitle { get; set; }
    public double TotalPrice { get; set; }
    public string UserId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public DateTime Created { get; set; }
}