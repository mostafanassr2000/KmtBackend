using KmtBackend.API.DTOs.User;
// User DTOs
using KmtBackend.API.Services.Interfaces;
// Service interfaces
using Microsoft.AspNetCore.Authorization;
// Authorization attributes
using Microsoft.AspNetCore.Mvc;
// MVC components
using System.Threading.Tasks;
// Async operations

namespace KmtBackend.API.Controllers
{
    // User management controller
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Only admins can access
    public class UserController : ControllerBase
    {
        // User service for business logic
        private readonly IUserService _userService;

        // Constructor with DI
        public UserController(IUserService userService)
        {
            // Store service reference
            _userService = userService;
        }

        // Get all users endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get all users
            var users = await _userService.GetAllUsersAsync();
            
            // Return user collection
            return Ok(users);
        }

        // Get user by ID endpoint
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Get specific user
            var user = await _userService.GetUserByIdAsync(id);
            
            // Return 404 if not found
            if (user == null)
                return NotFound();
                
            // Return user data
            return Ok(user);
        }

        // Create user endpoint
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            try
            {
                // Create new user
                var user = await _userService.CreateUserAsync(request);
                
                // Return created at action result
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                // Return error with message
                return BadRequest(new { message = ex.Message });
            }
        }

        // Update user endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            try
            {
                // Update existing user
                var user = await _userService.UpdateUserAsync(id, request);
                
                // Return updated user
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Handle not found
                if (ex.Message.Contains("not found"))
                    return NotFound(new { message = ex.Message });
                    
                // Handle other errors
                return BadRequest(new { message = ex.Message });
            }
        }

        // Delete user endpoint
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Attempt deletion
            var result = await _userService.DeleteUserAsync(id);
            
            // Return 404 if user not found
            if (!result)
                return NotFound();
                
            // Return success with no content
            return NoContent();
        }
    }
}
