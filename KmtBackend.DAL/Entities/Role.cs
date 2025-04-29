using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KmtBackend.DAL.Entities
{
    public class Role
    {
        /// <summary>
        /// Primary key for the role
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of the role (e.g., "Admin", "Editor")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Arabic Name of the role
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string NameAr { get; set; } = null!;

        /// <summary>
        /// Optional description of the role
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Optional Arabic description of the role
        /// </summary>
        [MaxLength(200)]
        public string? DescriptionAr { get; set; }

        /// <summary>
        /// Collection of users assigned this role
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        
        /// <summary>
        /// Collection of permissions assigned to this role
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
        
        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
