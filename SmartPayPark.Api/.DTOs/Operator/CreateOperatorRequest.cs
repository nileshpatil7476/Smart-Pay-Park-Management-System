
using System.ComponentModel.DataAnnotations;

namespace SmartPayPark.Api.DTOs.Operator;

public class CreateOperatorRequest
{
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string BusinessName { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string ContactName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string ContactEmail { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    [StringLength(500)]
    public string? BusinessAddress { get; set; }
}
