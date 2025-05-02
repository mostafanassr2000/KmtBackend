using KmtBackend.API.DTOs.User;
using KmtBackend.Models.DTOs.User;

namespace KmtBackend.BLL.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        
        Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest request);
        
        Task<bool> DeleteUserAsync(Guid id);
    }
}
