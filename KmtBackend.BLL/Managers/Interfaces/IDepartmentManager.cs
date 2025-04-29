using KmtBackend.API.DTOs.Department;

namespace KmtBackend.BLL.Managers.Interfaces
{
    public interface IDepartmentManager
    {
        Task<DepartmentResponse?> GetDepartmentByIdAsync(int id);
        
        Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();
        
        Task<DepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request);
        
        Task<DepartmentResponse> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request);
        
        Task<bool> DeleteDepartmentAsync(int id);
    }
}

