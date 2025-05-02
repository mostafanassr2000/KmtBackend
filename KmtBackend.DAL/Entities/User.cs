using System.Collections;
using System.ComponentModel.DataAnnotations;
// Validation attributes for properties
using System.ComponentModel.DataAnnotations.Schema;
// Database schema configuration

namespace KmtBackend.DAL.Entities
{
    // User entity represents both regular users and admins
    public class User
    {
        // Primary key for the user
        [Key]
        public Guid Id { get; set; }
        
        // Username for login purposes
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        
        // Email must be unique
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
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

        // Timestamp for record creation
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Last update timestamp
        public DateTime? UpdatedAt { get; set; }
    }
}
