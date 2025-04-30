using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Repositories.Interfaces;
using KmtBackend.Models.DTOs.Permission;
using MapsterMapper;

namespace KmtBackend.BLL.Managers
{
    /// <summary>
    /// Implementation of permission management operations
    /// </summary>
    public class PermissionManager : IPermissionManager
    {
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
        public PermissionManager(
            IPermissionRepository permissionRepository,
            IMapper mapper)
        {
            // Store permission repository for data access
            _permissionRepository = permissionRepository;
            // Store mapper for object transformations
            _mapper = mapper;
        }
        
        /// <summary>
        /// Gets all permissions
        /// </summary>
        public async Task<IEnumerable<PermissionResponse>> GetAllPermissionsAsync()
        {
            // Get all permissions from repository
            var permissions = await _permissionRepository.GetAllAsync();
            // Map to response DTOs and return
            return _mapper.Map<IEnumerable<PermissionResponse>>(permissions);
        }
        
        /// <summary>
        /// Gets a permission by ID
        /// </summary>
        public async Task<PermissionResponse?> GetPermissionByIdAsync(Guid id)
        {
            // Get permission by ID
            var permission = await _permissionRepository.GetByIdAsync(id);
            // Return null if not found
            if (permission == null) return null;
            // Map to response DTO and return
            return _mapper.Map<PermissionResponse>(permission);
        }
    }
}
