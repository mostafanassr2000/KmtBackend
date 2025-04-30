using KmtBackend.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KmtBackend.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Interface defining operations for role data access
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Gets all roles from the database
        /// </summary>
        /// <returns>Collection of all roles</returns>
        Task<IEnumerable<Role>> GetAllAsync();
        
        /// <summary>
        /// Gets a role by its unique identifier
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <returns>The role if found, otherwise null</returns>
        Task<Role?> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Gets a role by its name
        /// </summary>
        /// <param name="name">The role's name</param>
        /// <returns>The role if found, otherwise null</returns>
        Task<Role?> GetByNameAsync(string name);
        
        /// <summary>
        /// Creates a new role in the database
        /// </summary>
        /// <param name="role">The role to create</param>
        /// <returns>The created role with assigned ID</returns>
        Task<Role> CreateAsync(Role role);
        
        /// <summary>
        /// Updates an existing role in the database
        /// </summary>
        /// <param name="role">The role with updated properties</param>
        /// <returns>The updated role</returns>
        Task<Role> UpdateAsync(Role role);
        
        /// <summary>
        /// Deletes a role from the database
        /// </summary>
        /// <param name="id">The unique identifier of the role to delete</param>
        /// <returns>True if deleted successfully, otherwise false</returns>
        Task<bool> DeleteAsync(Guid id);
        
        /// <summary>
        /// Assigns a set of permissions to a role
        /// </summary>
        /// <param name="roleId">The role's unique identifier</param>
        /// <param name="permissions">Collection of permissions to assign</param>
        /// <returns>True if assigned successfully, otherwise false</returns>
        Task<bool> AssignPermissionsAsync(Guid roleId, IEnumerable<Permission> permissions);
        
        /// <summary>
        /// Checks if a role with the given name already exists
        /// </summary>
        /// <param name="name">The role name to check</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> NameExistsAsync(string name);
        
        /// <summary>
        /// Gets all roles with their permissions
        /// </summary>
        /// <returns>Collection of roles with permissions</returns>
        Task<IEnumerable<Role>> GetAllWithPermissionsAsync();
        
        /// <summary>
        /// Gets a role with its permissions
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <returns>The role with permissions if found, otherwise null</returns>
        Task<Role?> GetWithPermissionsAsync(Guid id);
    }
}
