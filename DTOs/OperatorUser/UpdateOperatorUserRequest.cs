
using System.ComponentModel.DataAnnotations;

namespace SmartPayPark.Api.DTOs.OperatorUser;

public class UpdateOperatorUserRequest
{
    [Required]
    [RegularExpression("Manager|Attendant")]
    public string OperatorRole { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
