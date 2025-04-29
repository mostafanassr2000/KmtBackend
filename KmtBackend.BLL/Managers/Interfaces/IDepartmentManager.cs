using KmtBackend.API.DTOs.Department;

namespace KmtBackend.BLL.Managers.Interfaces
{
    public interface IDepartmentManager
    {
        Task<DepartmentResponse?> GetDepartmentByIdAsync(Guid id);
        
        Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();
        
        Task<DepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request);
        
        Task<DepartmentResponse> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequest request);
        
        Task<bool> DeleteDepartmentAsync(Guid id);
    }
}

