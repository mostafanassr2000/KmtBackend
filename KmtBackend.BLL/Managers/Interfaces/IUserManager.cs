using KmtBackend.Models.DTOs.Common;
using KmtBackend.Models.DTOs.User;

namespace KmtBackend.BLL.Managers.Interfaces
{
    public interface IUserManager
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        Task<UserResponse?> GetUserByIdAsync(Guid id, Guid currentUserId);
        
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();

        Task<PaginatedResult<UserResponse>> GetAllUsersPaginatedAsync(PaginationQuery pagination);
        Task<PaginatedResult<UserResponse>> GetAllUsersPaginatedAsync(PaginationQuery pagination, Guid currentUserId);

        Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest request);

        Task<UserResponse> UpdateUserPasswordAsync(Guid id, UpdateUserPasswordRequest request);

        Task<bool> DeleteUserAsync(Guid id);
    }
}
