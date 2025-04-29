using KmtBackend.API.DTOs.User;

namespace KmtBackend.BLL.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        
        Task<UserResponse?> GetUserByIdAsync(int id);
        
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        
        Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request);
        
        Task<bool> DeleteUserAsync(int id);
    }
}
