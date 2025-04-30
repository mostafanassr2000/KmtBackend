using KmtBackend.API.Attributes;
using KmtBackend.API.DTOs.User;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KmtBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userService;

        public UserController(IUserManager userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [RequirePermission(Permissions.ViewUsers)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            
            return Ok(users);
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.ViewUsers)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            
            if (user == null)
                return NotFound();
                
            return Ok(user);
        }

        [HttpPost]
        [RequirePermission(Permissions.CreateUsers)]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            try
            {
                var user = await _userService.CreateUserAsync(request);
                
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.UpdateUsers)]
        public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, request);
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new { message = ex.Message });
                    
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.DeleteUsers)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
    }
}
