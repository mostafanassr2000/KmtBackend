using System.ComponentModel.DataAnnotations;

namespace KmtBackend.DAL.Entities
{
    public class Title : BaseEntity
    {
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
    }
}
