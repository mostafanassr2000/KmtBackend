using KmtBackend.API.DTOs.Department;
using KmtBackend.Models.DTOs.Common;
using KmtBackend.Models.DTOs.Department;

namespace KmtBackend.BLL.Managers.Interfaces
{
    public interface IDepartmentManager
    {
        Task<DepartmentResponse?> GetDepartmentByIdAsync(Guid id);
        Task<DepartmentResponse?> GetDepartmentByIdAsync(Guid id, Guid currentUserId);

        Task<PaginatedResult<DepartmentResponse>> GetAllDepartmentsAsync(PaginationQuery pagination);
        Task<PaginatedResult<DepartmentResponse>> GetAllDepartmentsAsync(PaginationQuery pagination, Guid currentUserId);
        
        Task<DepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request);
        
        Task<DepartmentResponse> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequest request);
        
        Task<bool> DeleteDepartmentAsync(Guid id);
    }
}

