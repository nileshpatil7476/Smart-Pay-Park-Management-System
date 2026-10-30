using SmartPayPark.Api.Models.Enums;
using System;
using System.Net;
using System.Numerics;

namespace SmartPayPark.Api.Models
{
    public class Operator
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string BusinessEmail { get; set; } = string.Empty;
        public string BusinessPhone { get; set; } = string.Empty;
        public string? GSTIN { get; set; }
        public string Address { get; set; } = string.Empty;
        public OperatorStatus Status { get; set; } = OperatorStatus.Pending;
        public DateTime ReviewedAtUtc { get; set; } = DateTime.UtcNow;
        public string? RejectionReason { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; }
        public User User { get; set; } = null!;
    }
}
