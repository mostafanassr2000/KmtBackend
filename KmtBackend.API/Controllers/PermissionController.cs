using KmtBackend.API.Attributes;
using KmtBackend.API.Common;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Constants;
using KmtBackend.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KmtBackend.API.Controllers
{
    /// <summary>
    /// Controller for permission management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        /// <summary>
        /// Permission manager for business logic
        /// </summary>
        private readonly IPermissionManager _permissionManager;
        
        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="permissionManager">The permission manager service</param>
        public PermissionController(IPermissionManager permissionManager)
        {
            // Store permission manager
            _permissionManager = permissionManager;
        }
        
        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Collection of permissions</returns>
        [HttpGet]
        [RequirePermission(Permissions.ViewPermissions)]
        public async Task<IActionResult> GetAll()
        {
            // Get all permissions from manager
            var permissions = await _permissionManager.GetAllPermissionsAsync();
            // Return OK with permissions
            return Ok(new ResponseWrapper(permissions, "Retrieved Permissions Successfully.", true));
        }
        
        /// <summary>
        /// Gets a permission by ID
        /// </summary>
        /// <param name="id">The permission's unique identifier</param>
        /// <returns>The permission if found</returns>
        [HttpGet("{id:guid}")]
        [RequirePermission(Permissions.ViewPermissions)]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Get permission by ID
            var permission = await _permissionManager.GetPermissionByIdAsync(id);
            
            // Return 404 if not found
            if (permission == null)
                return NotFound(new ResponseWrapper(null, "Permission Not Found", false));

            // Return OK with permission
            return Ok(new ResponseWrapper(permission, "Retrieved Permission Successfully.", true));
        }
    }
}
