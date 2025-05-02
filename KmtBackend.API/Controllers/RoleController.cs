using KmtBackend.API.Attributes;
using KmtBackend.API.Common;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Constants;
using KmtBackend.DAL.Entities;
using KmtBackend.Models.DTOs.Role;
using Microsoft.AspNetCore.Mvc;

namespace KmtBackend.API.Controllers
{
    /// <summary>
    /// Controller for role management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        /// <summary>
        /// Role manager for business logic
        /// </summary>
        private readonly IRoleManager _roleManager;
        
        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="roleManager">The role manager service</param>
        public RoleController(IRoleManager roleManager)
        {
            // Store role manager
            _roleManager = roleManager;
        }
        
        /// <summary>
        /// Gets all roles
        /// </summary>
        /// <returns>Collection of roles</returns>
        [HttpGet]
        [RequirePermission(Permissions.ViewRoles)]
        public async Task<IActionResult> GetAll()
        {
            // Get all roles from manager
            var roles = await _roleManager.GetAllRolesAsync();
            // Return OK with roles
            return Ok(new ResponseWrapper(roles, "Retrieved Roles Successfully.", true));
        }
        
        /// <summary>
        /// Gets a role by ID
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <returns>The role if found</returns>
        [HttpGet("{id:guid}")]
        [RequirePermission(Permissions.ViewRoles)]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Get role by ID
            var role = await _roleManager.GetRoleByIdAsync(id);
            
            // Return 404 if not found
            if (role == null)
                return NotFound(new ResponseWrapper(null, "Role Not Found", false));

            // Return OK with role
            return Ok(new ResponseWrapper(role, "Retrieved Role Successfully.", true));
        }
        
        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="request">The role creation request</param>
        /// <returns>The created role</returns>
        [HttpPost]
        [RequirePermission(Permissions.CreateRoles)]
        public async Task<IActionResult> Create(CreateRoleRequest request)
        {
            try
            {
                // Create role through manager
                var role = await _roleManager.CreateRoleAsync(request);
                // Return created result with new role
                return CreatedAtAction(null, new ResponseWrapper(role, "Role Created Succesfully.", true));
            }
            catch (Exception ex)
            {
                // Return bad request with error message
                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }
        
        /// <summary>
        /// Updates an existing role
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <param name="request">The role update request</param>
        /// <returns>The updated role</returns>
        [HttpPut("{id:guid}")]
        [RequirePermission(Permissions.UpdateRoles)]
        public async Task<IActionResult> Update(Guid id, UpdateRoleRequest request)
        {
            try
            {
                // Update role through manager
                var role = await _roleManager.UpdateRoleAsync(id, request);
                // Return OK with updated role
                return Ok(new ResponseWrapper(role, "Updated Role Successfully.", true));
            }
            catch (Exception ex)
            {
                // Return not found if role doesn't exist
                if (ex.Message.Contains("not found"))
                    return NotFound(new ResponseWrapper(null, "Role Not Found", false));

                // Return bad request for other errors
                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }
        
        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <returns>204 No Content if successful</returns>
        [HttpDelete("{id:guid}")]
        [RequirePermission(Permissions.DeleteRoles)]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Attempt to delete role
            var result = await _roleManager.DeleteRoleAsync(id);
            
            // Return 404 if role not found
            if (!result)
                return NotFound(new ResponseWrapper(null, "Role Not Found", false));

            // Return 204 No Content on success
            return NoContent();
        }
        
        /// <summary>
        /// Assigns permissions to a role
        /// </summary>
        /// <param name="id">The role's unique identifier</param>
        /// <param name="request">The permission assignment request</param>
        /// <returns>The updated role</returns>
        [HttpPost("{id:guid}/permissions")]
        [RequirePermission(Permissions.AssignPermissions)]
        public async Task<IActionResult> AssignPermissions(Guid id, AssignPermissionsRequest request)
        {
            try
            {
                // Assign permissions through manager
                var role = await _roleManager.AssignPermissionsAsync(id, request);
                // Return OK with updated role
                return Ok(new ResponseWrapper(role, "Assigned Permissions Successfully.", true));
            }
            catch (Exception ex)
            {
                // Return not found if role doesn't exist
                if (ex.Message.Contains("not found"))
                    return NotFound(new ResponseWrapper(null, "Role Not Found", false));

                // Return bad request for other errors
                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }
    }
}
