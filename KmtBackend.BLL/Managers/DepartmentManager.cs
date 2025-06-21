using KmtBackend.API.DTOs.Department;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;
using KmtBackend.Models.DTOs.Common;
using KmtBackend.Models.DTOs.Department;
using MapsterMapper;

namespace KmtBackend.BLL.Managers
{
    public class DepartmentManager : IDepartmentManager
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentFilteringService _departmentFilteringService;
        private readonly IMapper _mapper;

        public DepartmentManager(
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository,
            IDepartmentFilteringService departmentFilteringService,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _departmentFilteringService = departmentFilteringService;
            _mapper = mapper;
        }

        public async Task<DepartmentResponse?> GetDepartmentByIdAsync(Guid id, Guid currentUserId)
        {
            var currentUser = await _userRepository.GetUserWithRolesAsync(currentUserId);
            if (currentUser == null) return null;

            // Check if user has access to this department
            if (!_departmentFilteringService.HasDepartmentAccess(currentUser, id))
                return null;

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) return null;
            
            var response = _mapper.Map<DepartmentResponse>(department);
            
            var users = await _userRepository.GetByDepartmentAsync(id);
            response.UserCount = users.Count();
            
            return response;
        }

        public async Task<PaginatedResult<DepartmentResponse>> GetAllDepartmentsAsync(PaginationQuery pagination, Guid currentUserId)
        {
            var currentUser = await _userRepository.GetUserWithRolesAsync(currentUserId);
            if (currentUser == null)
                return new PaginatedResult<DepartmentResponse> { Items = new List<DepartmentResponse>() };

            var accessibleDepartmentIds = _departmentFilteringService.GetAccessibleDepartmentIds(currentUser);
            
            // If empty, it means super admin (no filtering)
            if (!accessibleDepartmentIds.Any())
            {
                var departments = await _departmentRepository.GetAllAsync(pagination);
                var responses = _mapper.Map<IEnumerable<DepartmentResponse>>(departments.Items).ToList();
                
                var allUsers = await _userRepository.GetAllAsync();
                foreach (var response in responses)
                {
                    response.UserCount = allUsers.Count(u => u.DepartmentId == response.Id);
                }

                return new PaginatedResult<DepartmentResponse>
                {
                    Items = responses,
                    PageNumber = departments.PageNumber,
                    PageSize = departments.PageSize,
                    TotalRecords = departments.TotalRecords
                };
            }
            else
            {
                // Filter by accessible departments
                var departments = await _departmentRepository.GetByIdsAsync(accessibleDepartmentIds);
                var responses = _mapper.Map<IEnumerable<DepartmentResponse>>(departments).ToList();
                
                var allUsers = await _userRepository.GetAllAsync();
                foreach (var response in responses)
                {
                    response.UserCount = allUsers.Count(u => u.DepartmentId == response.Id);
                }

                // Apply pagination manually
                var totalCount = responses.Count;
                var items = responses
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToList();

                return new PaginatedResult<DepartmentResponse>
                {
                    Items = items,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalCount
                };
            }
        }

        // Keep existing methods for backward compatibility
        public async Task<DepartmentResponse?> GetDepartmentByIdAsync(Guid id)
        {
            return await GetDepartmentByIdAsync(id, Guid.Empty);
        }

        public async Task<PaginatedResult<DepartmentResponse>> GetAllDepartmentsAsync(PaginationQuery pagination)
        {
            return await GetAllDepartmentsAsync(pagination, Guid.Empty);
        }

        public async Task<DepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request)
        {
            // Validate head of department if provided
            if (request.HeadOfDepartmentId.HasValue)
            {
                var headOfDepartment = await _userRepository.GetByIdAsync(request.HeadOfDepartmentId.Value);
                if (headOfDepartment == null)
                {
                    throw new Exception("Head of department user not found");
                }
            }

            var department = new Department
            {
                Name = request.Name,
                NameAr = request.NameAr,
                Description = request.Description,
                DescriptionAr = request.DescriptionAr,
                HeadOfDepartmentId = request.HeadOfDepartmentId,
                CreatedAt = DateTime.UtcNow
            };
            
            var createdDepartment = await _departmentRepository.CreateAsync(department);
            
            var response = _mapper.Map<DepartmentResponse>(createdDepartment);
            
            response.UserCount = 0;
            
            return response;
        }

        public async Task<DepartmentResponse> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequest request)
        {
            var department = await _departmentRepository.GetByIdAsync(id) ?? throw new Exception("Department not found");

            // Validate head of department if provided
            if (request.HeadOfDepartmentId.HasValue)
            {
                var headOfDepartment = await _userRepository.GetByIdAsync(request.HeadOfDepartmentId.Value);
                if (headOfDepartment == null)
                {
                    throw new Exception("Head of department user not found");
                }
            }

            department.Name = request.Name ?? department.Name;
            department.NameAr = request.NameAr ?? department.NameAr;
            department.Description = request.Description ?? department.Description;
            department.DescriptionAr = request.DescriptionAr ?? department.DescriptionAr;
            department.HeadOfDepartmentId = request.HeadOfDepartmentId ?? department.HeadOfDepartmentId;
            department.UpdatedAt = DateTime.UtcNow;
            
            var updatedDepartment = await _departmentRepository.UpdateAsync(department);
            
            var response = _mapper.Map<DepartmentResponse>(updatedDepartment);

            response.UserCount = department.Users.Count;
            
            return response;
        }

        public async Task<bool> DeleteDepartmentAsync(Guid id)
        {
            var users = await _userRepository.GetByDepartmentAsync(id);
            
            if (users.Any())
            {
                throw new Exception("Cannot delete department with assigned users");
            }
            
            return await _departmentRepository.DeleteAsync(id);
        }
    }
}

