
using System.ComponentModel.DataAnnotations;

namespace SmartPayPark.Api.DTOs.OperatorUser;

public class InviteOperatorUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression("Manager|Attendant",
        ErrorMessage = "Role must be Manager or Attendant.")]
    public string OperatorRole { get; set; } = string.Empty;
}
