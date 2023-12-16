namespace PhotoZoneBooking.DAL.Entities;

public class Reservation : BaseEntity
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Title { get; set; }
    public double Total { get; set; }
    public DateTime Created { get; set; }
    public string Status { get; set; } = "Pending";
    public string UserId { get; set; }
    public int PhotoZoneId { get; set; }
    public virtual User User { get; set; }
    public virtual PhotoZone PhotoZone { get; set; }
}