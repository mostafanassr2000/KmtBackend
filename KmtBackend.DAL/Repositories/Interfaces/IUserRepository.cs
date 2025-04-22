using KmtBackend.DAL.Entities;

namespace KmtBackend.DAL.Repositories.Interfaces
{
    // User repository interface
    public interface IUserRepository
    {
        // Get user by ID
        Task<User?> GetByIdAsync(int id);
        
        // Get user by email for authentication
        Task<User?> GetByEmailAsync(string email);
        
        // Get user by username (alternative login)
        Task<User?> GetByUsernameAsync(string username);
        
        // Get all users (for admin dashboard)
        Task<IEnumerable<User>> GetAllAsync();
        
        // Get users by department
        Task<IEnumerable<User>> GetByDepartmentAsync(int departmentId);
        
        // Create new user
        Task<User> CreateAsync(User user);
        
        // Update existing user
        Task<User> UpdateAsync(User user);
        
        // Delete user by ID
        Task<bool> DeleteAsync(int id);
        
        // Check if email is already taken
        Task<bool> EmailExistsAsync(string email);
        
        // Check if username is already taken
        Task<bool> UsernameExistsAsync(string username);
    }
}
