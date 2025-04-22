using KmtBackend.API.DTOs.Auth;
// Authentication DTOs
using KmtBackend.API.Services.Interfaces;
// Service interfaces
using Microsoft.AspNetCore.Mvc;
// MVC components
using System.Threading.Tasks;
// Async operations

namespace KmtBackend.API.Controllers
{
    // Authentication controller
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Auth service for business logic
        private readonly IAuthService _authService;

        // Constructor injection
        public AuthController(IAuthService authService)
        {
            // Store service reference
            _authService = authService;
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                // Attempt login
                var response = await _authService.LoginAsync(request);
                
                // Return successful response
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return unauthorized with message
                return Unauthorized(new { message = ex.Message });
            }
        }

        // Validate token endpoint
        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            // Check if token is valid
            var isValid = await _authService.ValidateTokenAsync(token);
            
            // Return validity status
            return Ok(new { isValid });
        }
    }
}
