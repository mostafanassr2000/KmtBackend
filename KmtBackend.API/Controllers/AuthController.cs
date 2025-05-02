using KmtBackend.API.Common;
using KmtBackend.API.DTOs.Auth;
using KmtBackend.BLL.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KmtBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authService;

        public AuthController(IAuthManager authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                
                return Ok(new ResponseWrapper(response, "Logged In Successfully!", true));
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            var isValid = await _authService.ValidateTokenAsync(token);
            
            return Ok(new { isValid });
        }
    }
}
