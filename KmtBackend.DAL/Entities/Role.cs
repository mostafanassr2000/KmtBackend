using System.ComponentModel.DataAnnotations;

namespace KmtBackend.DAL.Entities
{
    public class Role : BaseEntity
    {
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
        public virtual ICollection<User> User { get; set; } = [];
        
        /// <summary>
        /// Collection of permissions assigned to this role
        /// </summary>
        public virtual ICollection<Permission> Permissions { get; set; } = [];
    }
}
