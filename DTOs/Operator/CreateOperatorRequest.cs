
using System.ComponentModel.DataAnnotations;

namespace SmartPayPark.Api.DTOs.Operator;

public class CreateOperatorRequest
{
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string BusinessName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string BusinessAddress { get; set; } = string.Empty;
}
