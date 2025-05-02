using System.ComponentModel.DataAnnotations;

namespace KmtBackend.DAL.Entities
{
    public class Title
    {
        /// <summary>
        /// Primary key for the title
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of title
        /// </summary>
        [MaxLength(50)]
        public string? Name { get; set; }

        /// <summary>
        /// Arabic name of title
        /// </summary>
        [MaxLength(50)]
        public string? NameAr { get; set; }

        /// <summary>
        /// Description of title
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Arabic Description of title
        /// </summary>
        [MaxLength(200)]
        public string? DescriptionAr { get; set; }

        public virtual ICollection<User> Users { get; set; } = [];

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
