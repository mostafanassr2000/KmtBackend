using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KmtBackend.DAL.Entities
{
    // User entity represents both regular users and admins
    public class User : BaseEntity
    {      
        // Username for login purposes
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        
        // Email must be unique
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        // PhoneNumber must be unique
        [Required]
        [MaxLength(13)]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        // Storing hashed password only
        [Required]
        public string PasswordHash { get; set; } = null!;

        /// <summary>
        /// Collection of roles assigned to this user
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; } = [];

        // Foreign key to Department
        public Guid? DepartmentId { get; set; }
        
        // Navigation property
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        // Foreign key to Title
        public Guid? TitleId { get; set; }

        // Navigation property
        [ForeignKey("TitleId")]
        public Title? Title { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        // Navigation property for leave balances
        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = [];

        // Navigation property for leave requests
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = [];

        /// <summary>
        /// Prior work experience in months before joining this company
        /// </summary>
        [Required]
        public int PriorWorkExperienceMonths { get; set; }

        /// <summary>
        /// Total work experience in months (calculated)
        /// </summary>
        [NotMapped]
        public int TotalWorkExperienceMonths =>
            PriorWorkExperienceMonths +
            ((DateTime.UtcNow.Year - HireDate.Year) * 12) +
            (DateTime.UtcNow.Month - HireDate.Month);

        /// <summary>
        /// Total work experience in years (calculated)
        /// </summary>
        [NotMapped]
        public int TotalWorkExperienceYears => TotalWorkExperienceMonths / 12;
    }
}
