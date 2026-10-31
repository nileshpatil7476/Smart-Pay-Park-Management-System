using System.ComponentModel.DataAnnotations;

namespace SmartPayPark.Application.Authentication.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(256, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [StringLength(128)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MinLength(10)]
        [StringLength(10)]
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
