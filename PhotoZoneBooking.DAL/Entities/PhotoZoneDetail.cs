namespace PhotoZoneBooking.DAL.Entities;

public class PhotoZoneDetail : BaseEntity
{
    public string Address { get; set; }
    public string Description { get; set; }
    public string StartProgram { get; set; }
    public string EndProgram { get; set; }
    public int PhotoZoneId { get; set; }
    public virtual PhotoZone PhotoZone { get; set; }
}