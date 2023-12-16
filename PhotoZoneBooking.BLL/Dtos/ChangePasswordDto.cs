using System.ComponentModel.DataAnnotations;

namespace PhotoZoneBooking.BLL.Dtos;

public class ChangePasswordDto
{
    [Required]
    [DataType(DataType.Password)]
    [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [RegularExpression(@"^(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", 
        ErrorMessage = "Password must contain: both upper and lowercase characters, at least on digit, one special character")]
    public string Password { get; set; }
}