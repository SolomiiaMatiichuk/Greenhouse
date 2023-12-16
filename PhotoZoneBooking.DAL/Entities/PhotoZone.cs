namespace PhotoZoneBooking.DAL.Entities;

public class PhotoZone : BaseEntity
{
    public int PricePerHour { get; set; }
    public string ImageUrl { get; set; }
    public string Title { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    public virtual PhotoZoneDetail PhotoZoneDetail { get; set; }
}