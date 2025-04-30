using KmtBackend.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KmtBackend.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Interface defining operations for permission data access
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// Gets all permissions from the database
        /// </summary>
        /// <returns>Collection of all permissions</returns>
        Task<IEnumerable<Permission>> GetAllAsync();
        
        /// <summary>
        /// Gets a permission by its unique identifier
        /// </summary>
        /// <param name="id">The permission's unique identifier</param>
        /// <returns>The permission if found, otherwise null</returns>
        Task<Permission?> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Gets a permission by its code
        /// </summary>
        /// <param name="code">The permission's code</param>
        /// <returns>The permission if found, otherwise null</returns>
        Task<Permission?> GetByCodeAsync(string code);
        
        /// <summary>
        /// Gets permissions by their IDs
        /// </summary>
        /// <param name="ids">Collection of permission IDs</param>
        /// <returns>Collection of found permissions</returns>
        Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<Guid> ids);
    }
}
