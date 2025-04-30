using KmtBackend.Models.DTOs.Role;

namespace KmtBackend.BLL.Managers.Interfaces
{
    /// <summary>
    /// Interface for role management operations
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// Gets all roles
        /// </summary>
        /// <returns>Collection of role responses</returns>
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync();
        
        /// <summary>
        /// Gets a role by ID
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <returns>Role response if found, otherwise null</returns>
        Task<RoleResponse?> GetRoleByIdAsync(Guid id);
        
        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="request">Create role request with role data</param>
        /// <returns>The created role response</returns>
        Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
        
        /// <summary>
        /// Updates an existing role
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <param name="request">Update role request with new data</param>
        /// <returns>The updated role response</returns>
        Task<RoleResponse> UpdateRoleAsync(Guid id, UpdateRoleRequest request);
        
        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <returns>True if deleted successfully, otherwise false</returns>
        Task<bool> DeleteRoleAsync(Guid id);
        
        /// <summary>
        /// Assigns permissions to a role
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <param name="request">Request with permission IDs</param>
        /// <returns>The updated role with permissions</returns>
        Task<RoleResponse> AssignPermissionsAsync(Guid id, AssignPermissionsRequest request);
    }
}
