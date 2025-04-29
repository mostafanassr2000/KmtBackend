using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KmtBackend.DAL.Entities
{
    /// <summary>
    /// Junction entity for User and Role many-to-many relationship
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Foreign key to User
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Foreign key to Role
        /// </summary>
        public Guid RoleId { get; set; }
        
        /// <summary>
        /// Navigation property to User
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        
        /// <summary>
        /// Navigation property to Role
        /// </summary>
        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;
        
        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
