using System.ComponentModel.DataAnnotations;

namespace PhotoZoneBooking.BLL.Dtos;

public class PhotoZoneDto
{
    public int Id { get; set; }
    public int PhotoZoneDetailId { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public string Address { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public int PricePerHour { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public string ImageUrl { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public string Title { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public string Description { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public string StartProgram { get; set; }
    [Required(ErrorMessage = "That field is required!")]
    public string EndProgram { get; set; }
}