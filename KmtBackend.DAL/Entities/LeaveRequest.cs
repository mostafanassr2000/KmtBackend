using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KmtBackend.DAL.Entities
{
    public class LeaveRequest : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [Required]
        public Guid LeaveTypeId { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public LeaveType LeaveType { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // Total requested days
        [Required]
        public int Days { get; set; }

        // Status: Pending, Approved, Rejected, Canceled
        [Required]
        public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

        // Manager who processed the request
        public Guid? ProcessedById { get; set; }

        [ForeignKey(nameof(ProcessedById))]
        public User? ProcessedBy { get; set; }

        // When the request was processed
        public DateTime? ProcessedDate { get; set; }

        // Reason provided by manager for rejection (if applicable)
        [MaxLength(500)]
        public string? RejectionReason { get; set; }
    }
    public enum LeaveRequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Canceled
    }
}
