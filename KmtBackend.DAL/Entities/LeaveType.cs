using System.ComponentModel.DataAnnotations;

namespace KmtBackend.DAL.Entities
{
    public class LeaveType : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string NameAr { get; set; } = null!;

        [MaxLength(250)]
        public string? Description { get; set; }

        [MaxLength(250)]
        public string? DescriptionAr { get; set; }

        // Whether this type increases with seniority (like annual leave)
        public bool IsSeniorityBased { get; set; }

        // Whether leave balance of this type can carry over to next year
        public bool AllowCarryOver { get; set; }

        // Navigation properties
        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = [];
    }
}
