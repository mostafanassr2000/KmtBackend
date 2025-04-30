using KmtBackend.DAL.Context;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KmtBackend.DAL.Repositories
{
    /// <summary>
    /// Implementation of permission repository
    /// </summary>
    public class PermissionRepository : IPermissionRepository
    {
        /// <summary>
        /// Database context for data access
        /// </summary>
        private readonly KmtDbContext _context;
        
        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">The database context</param>
        public PermissionRepository(KmtDbContext context)
        {
            // Store the context for database operations
            _context = context;
        }
        
        /// <summary>
        /// Gets all permissions
        /// </summary>
        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            // Return all permissions ordered by code
            return await _context.Permissions
                .OrderBy(p => p.Code)
                .ToListAsync();
        }
        
        /// <summary>
        /// Gets a permission by ID
        /// </summary>
        public async Task<Permission?> GetByIdAsync(Guid id)
        {
            // Find and return permission with matching ID
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        /// <summary>
        /// Gets a permission by code
        /// </summary>
        public async Task<Permission?> GetByCodeAsync(string code)
        {
            // Find and return permission with matching code (case-insensitive)
            return await _context.Permissions
                .FirstOrDefaultAsync(p => p.Code.ToLower() == code.ToLower());
        }
        
        /// <summary>
        /// Gets permissions by their IDs
        /// </summary>
        public async Task<IEnumerable<Permission>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            // Convert IDs to a list for the query
            var idsList = ids.ToList();
            
            // Return permissions that match any of the provided IDs
            return await _context.Permissions
                .Where(p => idsList.Contains(p.Id))
                .ToListAsync();
        }
    }
}
