using KmtBackend.API.DTOs.Department;
// Department DTOs
using System.Collections.Generic;
// Collections
using System.Threading.Tasks;
// Async operations

namespace KmtBackend.API.Services.Interfaces
{
    // Department service interface
    public interface IDepartmentService
    {
        // Get department by ID
        Task<DepartmentResponse?> GetDepartmentByIdAsync(int id);
        
        // Get all departments
        Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();
        
        // Create new department
        Task<DepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request);
        
        // Update existing department
        Task<DepartmentResponse> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request);
        
        // Delete department
        Task<bool> DeleteDepartmentAsync(int id);
    }
}

