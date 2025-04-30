using KmtBackend.Models.DTOs.Permission;

namespace KmtBackend.BLL.Managers.Interfaces
{
    /// <summary>
    /// Interface for permission management operations
    /// </summary>
    public interface IPermissionManager
    {
        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Collection of permission responses</returns>
        Task<IEnumerable<PermissionResponse>> GetAllPermissionsAsync();
        
        /// <summary>
        /// Gets a permission by ID
        /// </summary>
        /// <param name="id">The permission's unique identifier</param>
        /// <returns>Permission response if found, otherwise null</returns>
        Task<PermissionResponse?> GetPermissionByIdAsync(Guid id);
    }
}
