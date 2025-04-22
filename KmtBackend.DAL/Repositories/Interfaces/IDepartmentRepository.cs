using KmtBackend.DAL.Entities;
// Domain models
using System.Collections.Generic;
// Collection types
using System.Threading.Tasks;
// Async operations

namespace KmtBackend.DAL.Repositories.Interfaces
{
    // Department repository interface
    public interface IDepartmentRepository
    {
        // Get department by ID
        Task<Department?> GetByIdAsync(int id);
        
        // Get all departments
        Task<IEnumerable<Department>> GetAllAsync();
        
        // Create new department
        Task<Department> CreateAsync(Department department);
        
        // Update existing department
        Task<Department> UpdateAsync(Department department);
        
        // Delete department
        Task<bool> DeleteAsync(int id);
    }
}

