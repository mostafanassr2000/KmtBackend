using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;
using KmtBackend.Models.DTOs.Role;
using MapsterMapper;

namespace KmtBackend.BLL.Managers
{
    /// <summary>
    /// Implementation of role management operations
    /// </summary>
    public class RoleManager : IRoleManager
    {
        /// <summary>
        /// Repository for role data access
        /// </summary>
        private readonly IRoleRepository _roleRepository;
        
        /// <summary>
        /// Repository for permission data access
        /// </summary>
        private readonly IPermissionRepository _permissionRepository;
        
        /// <summary>
        /// Mapper for object transformations
        /// </summary>
        private readonly IMapper _mapper;
        
        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        public RoleManager(
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IMapper mapper)
        {
            // Store role repository for data access
            _roleRepository = roleRepository;
            // Store permission repository for data access
            _permissionRepository = permissionRepository;
            // Store mapper for object transformations
            _mapper = mapper;
        }
        
        /// <summary>
        /// Gets all roles
        /// </summary>
        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync()
        {
            // Get all roles with permissions included
            var roles = await _roleRepository.GetAllWithPermissionsAsync();
            // Map to response DTOs and return
            return _mapper.Map<IEnumerable<RoleResponse>>(roles);
        }
        
        /// <summary>
        /// Gets a role by ID
        /// </summary>
        public async Task<RoleResponse?> GetRoleByIdAsync(Guid id)
        {
            // Get role with permissions
            var role = await _roleRepository.GetWithPermissionsAsync(id);
            // Return null if not found
            if (role == null) return null;
            // Map to response DTO and return
            return _mapper.Map<RoleResponse>(role);
        }
        
        /// <summary>
        /// Creates a new role
        /// </summary>
        public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
        {
            // Check if name already exists
            if (await _roleRepository.NameExistsAsync(request.Name))
            {
                // Throw exception if name exists
                throw new Exception("Role name already exists");
            }

            // Create role entity
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                NameAr = request.NameAr,
                Description = request.Description,
                DescriptionAr = request.DescriptionAr,
                CreatedAt = DateTime.UtcNow
            };

            // If permissions are provided, assign them
            if (request.PermissionIds != null && request.PermissionIds.Any())
            {
                var rolePermissions = await _permissionRepository.GetByIdsAsync(request.PermissionIds);
                // Assign permissions to role
                role.Permissions = rolePermissions.ToList();
            }

            // Add role to database
            var createdRole = await _roleRepository.CreateAsync(role);
            
            // Map to response DTO and return
            return _mapper.Map<RoleResponse>(createdRole);
        }
        
        /// <summary>
        /// Updates an existing role
        /// </summary>
        public async Task<RoleResponse> UpdateRoleAsync(Guid id, UpdateRoleRequest request)
        {
            // Get existing role
            var role = await _roleRepository.GetByIdAsync(id) ?? throw new Exception("Role not found");

            // Check if new name is different and already exists
            if (request.Name != role.Name && await _roleRepository.NameExistsAsync(request.Name))
            {
                // Throw exception if name exists
                throw new Exception("Role name already exists");
            }
            
            // Update properties
            role.Name = request.Name;
            role.NameAr = request.NameAr;
            role.Description = request.Description;
            role.DescriptionAr = request.DescriptionAr;
            role.UpdatedAt = DateTime.UtcNow;         
            
            // If permissions are provided, update them
            if (request.PermissionIds != null)
            {
                var newPermissions = await _permissionRepository.GetByIdsAsync(request.PermissionIds);
                // Assign new permissions
                role.Permissions = newPermissions.ToList();
            }

            // Save changes
            var updatedRole = request.PermissionIds == null
                ? await _roleRepository.UpdateAsync(role)
                : await _roleRepository.GetWithPermissionsAsync(id);

            // Map to response DTO and return
            return _mapper.Map<RoleResponse>(updatedRole);
        }
        
        /// <summary>
        /// Deletes a role
        /// </summary>
        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            // Attempt to delete role and return result
            return await _roleRepository.DeleteAsync(id);
        }
        
        /// <summary>
        /// Assigns permissions to a role
        /// </summary>
        public async Task<RoleResponse> AssignPermissionsAsync(Guid id, AssignPermissionsRequest request)
        {
            // Check if role exists
            var role = await _roleRepository.GetByIdAsync(id);
            // Throw exception if not found
            if (role == null)
            {
                throw new Exception("Role not found");
            }
            
            // Check if permissions exist
            if (request.PermissionIds.Any())
            {
                // Get all permissions that match the requested IDs
                var permissions = await _permissionRepository.GetByIdsAsync(request.PermissionIds);
                // Get actual permission IDs that were found
                var foundPermissionIds = permissions.Select(p => p.Id).ToList();
                
                // Check if any requested permissions weren't found
                var notFoundIds = request.PermissionIds.Except(foundPermissionIds).ToList();
                if (notFoundIds.Any())
                {
                    // Throw exception with list of invalid IDs
                    throw new Exception($"Some permissions were not found: {string.Join(", ", notFoundIds)}");
                }
                // Assign permissions to role
                await _roleRepository.AssignPermissionsAsync(id, permissions.ToList());
            }
            
            // Get updated role with permissions
            var updatedRole = await _roleRepository.GetWithPermissionsAsync(id);
            // Throw exception if role not found (shouldn't happen)
            if (updatedRole == null)
            {
                throw new Exception("Role not found after update");
            }
            
            // Map to response DTO and return
            return _mapper.Map<RoleResponse>(updatedRole);
        }
    }
}
