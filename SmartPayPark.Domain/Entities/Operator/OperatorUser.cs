using SmartPayPark.Domain.Entities.Enums;
using SmartPayPark.Domain.Entities.User;

namespace SmartPayPark.Domain.Entities;

public class OperatorUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OperatorId { get; set; }

    public Guid UserId { get; set; }

    public OperatorRole OperatorRole { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid? InvitedByUserId { get; set; }

    public DateTime? JoinedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Operator Operator { get; set; } = null!;

    public User.User User { get; set; } = null!;

    public User.User? InvitedByUser { get; set; }
}