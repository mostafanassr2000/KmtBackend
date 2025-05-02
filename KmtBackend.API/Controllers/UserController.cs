using KmtBackend.API.Attributes;
using KmtBackend.API.Common;
using KmtBackend.API.DTOs.User;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Constants;
using KmtBackend.DAL.Entities;
using KmtBackend.Models.DTOs.User;
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

            return Ok(new ResponseWrapper(users, "Retrieved Users Successfully.", true));
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.ViewUsers)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            
            if (user == null)
                return NotFound(new ResponseWrapper(null, "User Not Found", false));

            return Ok(new ResponseWrapper(user, "Retrieved User Successfully.", true));
        }

        [HttpPost]
        [RequirePermission(Permissions.CreateUsers)]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            try
            {
                var user = await _userService.CreateUserAsync(request);

                return CreatedAtAction(null, new ResponseWrapper(user, "User Created Succesfully.", true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.UpdateUsers)]
        public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, request);

                return Ok(new ResponseWrapper(user, "Updated User Successfully.", true));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new ResponseWrapper(null, "User Not Found", false));

                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.DeleteUsers)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            
            if (!result)
                return NotFound(new ResponseWrapper(null, "User Not Found", false));

            return NoContent();
        }
    }
}
