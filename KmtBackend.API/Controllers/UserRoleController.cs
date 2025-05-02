using KmtBackend.API.Attributes;
using KmtBackend.API.Common;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Constants;
using KmtBackend.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace KmtBackend.API.Controllers
{
    /// <summary>
    /// Controller for managing user-role assignments
    /// </summary>
    [ApiController]
    [Route("api/User/{userId:guid}/roles")]
    public class UserRoleController : ControllerBase
    {
        /// <summary>
        /// User-role manager for business logic
        /// </summary>
        private readonly IUserRoleManager _userRoleManager;
        
        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="userRoleManager">The user-role manager service</param>
        public UserRoleController(IUserRoleManager userRoleManager)
        {
            // Store user-role manager
            _userRoleManager = userRoleManager;
        }
        
        /// <summary>
        /// Gets all roles for a user
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>Collection of roles assigned to the user</returns>
        [HttpGet]
        [RequirePermission(Permissions.ViewRoles)]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            // Get user roles from manager
            var roles = await _userRoleManager.GetUserRolesAsync(userId);
            // Return OK with roles
            return Ok(new ResponseWrapper(roles, "Retrieved Roles Successfully.", true));
        }
        
        /// <summary>
        /// Assigns roles to a user
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <param name="request">The role assignment request</param>
        /// <returns>The updated user with roles</returns>
        [HttpPost]
        [RequirePermission(Permissions.AssignRoles)]
        public async Task<IActionResult> AssignRoles(Guid userId, AssignRolesRequest request)
        {
            try
            {
                // Assign roles through manager
                var user = await _userRoleManager.AssignRolesToUserAsync(userId, request);
                // Return OK with updated user
                return Ok(new ResponseWrapper(user, "Assigned Roles Successfully.", true));
            }
            catch (Exception ex)
            {
                // Return not found if user doesn't exist
                if (ex.Message.Contains("not found"))
                    return NotFound(new ResponseWrapper(null, "User Not Found", false));

                // Return bad request for other errors
                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }
    }
}
