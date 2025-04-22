using System.ComponentModel.DataAnnotations;
// Data annotations for validation
using System.Collections.Generic;
// Collections for relationships

namespace KmtBackend.DAL.Entities
{
    // Department entity for organizational structure
    public class Department
    {
        // Primary key
        [Key]
        public int Id { get; set; }
        
        // Department name
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        
        // Department name in Arabic
        [MaxLength(100)]
        public string? NameAr { get; set; }
        
        // Optional description
        [MaxLength(500)]
        public string? Description { get; set; }
        
        // Collection of users in this department
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        
        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
