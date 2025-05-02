using System.ComponentModel.DataAnnotations;

namespace KmtBackend.DAL.Entities
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        
        [MaxLength(100)]
        public string? NameAr { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
        [MaxLength(500)]
        public string? DescriptionAr { get; set; }

        public virtual ICollection<User> Users { get; set; } = [];
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
