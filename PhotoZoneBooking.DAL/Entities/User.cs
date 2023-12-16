using Microsoft.AspNetCore.Identity;

namespace PhotoZoneBooking.DAL.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
}