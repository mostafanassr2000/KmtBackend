using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KmtBackend.Models.DTOs.Role
{
    /// <summary>
    /// Request for updating an existing role
    /// </summary>
    public class UpdateRoleRequest
    {
        /// <summary>
        /// Updated role name
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Updated role name in Arabic
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string NameAr { get; set; } = null!;
        
        /// <summary>
        /// Updated role description
        /// </summary>
        [StringLength(200)]
        public string? Description { get; set; }
        
        /// <summary>
        /// Updated role description in Arabic
        /// </summary>
        [StringLength(200)]
        public string? DescriptionAr { get; set; }
        
        /// <summary>
        /// Updated list of permission IDs to assign
        /// </summary>
        public ICollection<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}
