using System.ComponentModel.DataAnnotations;
using PhotoZoneBooking.BLL.Validators;

namespace PhotoZoneBooking.BLL.Dtos;

public class ReservationDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public DateTime Start { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    [AttributeGreaterThan("Start", ErrorMessage = "Reservation`s end date should be after the start date")]
    public DateTime End { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public string Title { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public double Total { get; set; }
    public string? Status { get; set; }
    public string? UserId { get; set; }
    public int PhotoZoneId { get; set; }
}