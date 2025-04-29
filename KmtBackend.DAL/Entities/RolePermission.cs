using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KmtBackend.DAL.Entities
{
    /// <summary>
    /// Junction entity for Role and Permission many-to-many relationship
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Foreign key to Role
        /// </summary>
        public Guid RoleId { get; set; }
        
        /// <summary>
        /// Foreign key to Permission
        /// </summary>
        public Guid PermissionId { get; set; }
        
        /// <summary>
        /// Navigation property to Role
        /// </summary>
        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;
        
        /// <summary>
        /// Navigation property to Permission
        /// </summary>
        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; } = null!;
        
        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
