using KmtBackend.DAL.Context;
using KmtBackend.DAL.Entities;
// Domain models
using KmtBackend.DAL.Repositories.Interfaces;
// Repository interfaces
using Microsoft.EntityFrameworkCore;
// EF Core functionality
using System.Collections.Generic;
// Collections
using System.Threading.Tasks;
// Asynchronous operations

namespace KmtBackend.DAL.Repositories
{
    // Implementation of user repository
    public class UserRepository : IUserRepository
    {
        // Database context
        private readonly KmtDbContext _context;
        
        // Constructor with dependency injection
        public UserRepository(KmtDbContext context)
        {
            // Store context for data operations
            _context = context;
        }

        // Get user by ID with department info
        public async Task<User?> GetByIdAsync(Guid id)
        {
            // Query with included department
            return await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Find user by email for authentication
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.Email.ToLower() == email.ToLower())
                // Include user roles and role information
                .Include(u => u.Roles)
                .ThenInclude(rp => rp.Permissions)
                .Include(u => u.Department)
                .FirstOrDefaultAsync();
        }

        // Find user by username (alternative login)
        public async Task<User?> GetByUsernameAsync(string username)
        {
            // Case-insensitive username comparison
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        // Get all users for admin dashboard
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Return all users with departments
            return await _context.Users
                .Include(u => u.Department)
                .ToListAsync();
        }

        // Get users filtered by department
        public async Task<IEnumerable<User>> GetByDepartmentAsync(Guid departmentId)
        {
            // Filter by department ID
            return await _context.Users
                .Where(u => u.DepartmentId == departmentId)
                .ToListAsync();
        }

        // Get users filtered by department
        public async Task<IEnumerable<User>> GetByTitleAsync(Guid titleId)
        {
            // Filter by title ID
            return await _context.Users
                .Where(u => u.TitleId == titleId)
                .ToListAsync();
        }

        // Create new user
        public async Task<User> CreateAsync(User user)
        {
            // Add user to context
            await _context.Users.AddAsync(user);
            // Save changes to database
            await _context.SaveChangesAsync();
            // Return created user with ID
            return user;
        }

        // Update existing user
        public async Task<User> UpdateAsync(User user)
        {
            // Mark entity as modified
            _context.Users.Update(user);
            // Set update timestamp
            user.UpdatedAt = DateTime.UtcNow;
            // Save to database
            await _context.SaveChangesAsync();
            // Return updated entity
            return user;
        }

        // Delete user by ID
        public async Task<bool> DeleteAsync(Guid id)
        {
            // Find user to delete
            var user = await _context.Users.FindAsync(id);
            // Return false if user not found
            if (user == null) return false;
            
            // Remove user from context
            _context.Users.Remove(user);
            // Save changes and return success
            return await _context.SaveChangesAsync() > 0;
        }

        // Check if email exists (for validation)
        public async Task<bool> EmailExistsAsync(string email)
        {
            // Case-insensitive email check
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        // Check if username exists (for validation)
        public async Task<bool> UsernameExistsAsync(string username)
        {
            // Case-insensitive username check
            return await _context.Users
                .AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        /// <summary>
        /// Gets a user with their roles included
        /// </summary>
        public async Task<User?> GetUserWithRolesAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
