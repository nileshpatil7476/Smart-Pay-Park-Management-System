
namespace SmartPayPark.Api.DTOs.OperatorUser;

public class OperatorUserResponse
{
    public Guid Id { get; set; }

    public Guid OperatorId { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string OperatorRole { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime? JoinedAtUtc { get; set; }
}
