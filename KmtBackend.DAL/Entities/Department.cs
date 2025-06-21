using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KmtBackend.DAL.Entities
{
    public class Department : BaseEntity
    {
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string NameAr { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; } = null!;
        [MaxLength(500)]
        public string DescriptionAr { get; set; } = null!;

        // Head of Department relationship
        public Guid? HeadOfDepartmentId { get; set; }
        
        [ForeignKey("HeadOfDepartmentId")]
        public virtual User? HeadOfDepartment { get; set; }

        public virtual ICollection<User> Users { get; set; } = [];
    }
}
