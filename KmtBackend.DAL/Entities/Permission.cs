using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace KmtBackend.DAL.Entities
{
    public class Permission
    {
        /// <summary>
        /// Primary key for the permission
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Permission code in format "resource.action" (e.g., "users.create")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = null!;
        
        /// <summary>
        /// Description of what this permission allows
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Arabic Description of what this permission allows
        /// </summary>
        [MaxLength(200)]
        public string? DescriptionAr { get; set; }

        /// <summary>
        /// Collection of roles that have this permission
        /// </summary>
        public virtual ICollection<Role> Role { get; set; } = [];
        
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
