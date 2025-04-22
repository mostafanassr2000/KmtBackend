using KmtBackend.API.DTOs.User;
// User-related DTOs
using System.Collections.Generic;
// Collections for returning multiple users
using System.Threading.Tasks;
// Async operations

namespace KmtBackend.API.Services.Interfaces
{
    // User management service interface
    public interface IUserService
    {
        // Create new user (for admin)
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        
        // Get user by ID
        Task<UserResponse?> GetUserByIdAsync(int id);
        
        // Get all users (for admin dashboard)
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        
        // Update user
        Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request);
        
        // Delete user
        Task<bool> DeleteUserAsync(int id);
    }
}
