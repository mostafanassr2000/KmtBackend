using KmtBackend.API.DTOs.Auth;
// Authentication DTOs
using System.Threading.Tasks;
// Async support

namespace KmtBackend.API.Services.Interfaces
{
    // Authentication service interface
    public interface IAuthService
    {
        // Login with credentials
        Task<LoginResponse> LoginAsync(LoginRequest request);
        
        // Validate if token is still valid
        Task<bool> ValidateTokenAsync(string token);
    }
}
