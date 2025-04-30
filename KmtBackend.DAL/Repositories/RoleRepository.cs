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
    /// Implementation of role repository
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        /// <summary>
        /// Database context for data access
        /// </summary>
        private readonly KmtDbContext _context;
        
        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="context">The database context</param>
        public RoleRepository(KmtDbContext context)
        {
            // Store the context for database operations
            _context = context;
        }
        
        /// <summary>
        /// Gets all roles from database
        /// </summary>
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            // Return all roles ordered by name
            return await _context.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();
        }
        
        /// <summary>
        /// Gets a role by ID
        /// </summary>
        public async Task<Role?> GetByIdAsync(Guid id)
        {
            // Find and return role with matching ID
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        
        /// <summary>
        /// Gets a role by name
        /// </summary>
        public async Task<Role?> GetByNameAsync(string name)
        {
            // Find and return role with matching name (case-insensitive)
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
        }
        
        /// <summary>
        /// Creates a new role
        /// </summary>
        public async Task<Role> CreateAsync(Role role)
        {
            // Add the role to context
            await _context.Roles.AddAsync(role);
            // Save changes to database
            await _context.SaveChangesAsync();
            // Return the created role with ID
            return role;
        }
        
        /// <summary>
        /// Updates an existing role
        /// </summary>
        public async Task<Role> UpdateAsync(Role role)
        {
            // Mark entity as modified
            _context.Roles.Update(role);
            // Set update timestamp
            role.UpdatedAt = DateTime.UtcNow;
            // Save changes to database
            await _context.SaveChangesAsync();
            // Return the updated role
            return role;
        }
        
        /// <summary>
        /// Deletes a role by ID
        /// </summary>
        public async Task<bool> DeleteAsync(Guid id)
        {
            // Find the role by ID
            var role = await _context.Roles.FindAsync(id);
            // Return false if role not found
            if (role == null) return false;
            
            // Remove role from context
            _context.Roles.Remove(role);
            // Save changes and return true if successful
            return await _context.SaveChangesAsync() > 0;
        }
        
        /// <summary>
        /// Assigns permissions to a role
        /// </summary>
        public async Task<bool> AssignPermissionsAsync(Guid roleId, IEnumerable<Permission> permissions)
        {
            // Get the role with existing permissions
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == roleId);
            
            // Return false if role not found
            if (role == null) return false;

            role.Permissions = [.. permissions];
            
            // Save changes and return true if successful
            return await _context.SaveChangesAsync() > 0;
        }
        
        /// <summary>
        /// Checks if a role name already exists
        /// </summary>
        public async Task<bool> NameExistsAsync(string name)
        {
            // Check if any role has this name (case-insensitive)
            return await _context.Roles
                .AnyAsync(r => r.Name.ToLower() == name.ToLower());
        }
        
        /// <summary>
        /// Gets all roles with their permissions
        /// </summary>
        public async Task<IEnumerable<Role>> GetAllWithPermissionsAsync()
        {
            // Return all roles with permissions included
            return await _context.Roles
                .Include(r => r.Permissions)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }
        
        /// <summary>
        /// Gets a role with its permissions
        /// </summary>
        public async Task<Role?> GetWithPermissionsAsync(Guid id)
        {
            // Find and return role with permissions
            return await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
