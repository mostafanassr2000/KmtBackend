using KmtBackend.API.DTOs.Auth;

namespace KmtBackend.BLL.Managers.Interfaces
{
    public interface IAuthManager
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        
        Task<bool> ValidateTokenAsync(string token);
    }
}
