using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KmtBackend.DAL.Entities
{
    public class Mission : BaseEntity
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string DescriptionAr { get; set; } = null!;

        [Required]
        public DateTime MissionDate { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        public TimeSpan? EndTime { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = null!;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        [Required]
        public Guid CreatedById { get; set; }
        
        [ForeignKey(nameof(CreatedById))]
        public User CreatedBy { get; set; } = null!;
        
        public virtual ICollection<User> Users { get; set; } = [];
    }
}