using KmtBackend.DAL.Context;
// Database context
using KmtBackend.DAL.Entities;
// Domain models
using KmtBackend.DAL.Repositories.Interfaces;
// Repository interfaces
using Microsoft.EntityFrameworkCore;
// EF Core functionality
using System.Collections.Generic;
// Collections
using System.Threading.Tasks;
// Async operations

namespace KmtBackend.DAL.Repositories
{
    // Department repository implementation
    public class DepartmentRepository : IDepartmentRepository
    {
        // Database context
        private readonly KmtDbContext _context;
        
        // Constructor with DI
        public DepartmentRepository(KmtDbContext context)
        {
            // Store context
            _context = context;
        }

        // Get department by ID
        public async Task<Department?> GetByIdAsync(Guid id)
        {
            // Query database for department
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        // Get all departments
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            // Return all departments
            return await _context.Departments.ToListAsync();
        }

        // Create new department
        public async Task<Department> CreateAsync(Department department)
        {
            // Add to context
            await _context.Departments.AddAsync(department);
            // Save changes
            await _context.SaveChangesAsync();
            // Return created department
            return department;
        }

        // Update existing department
        public async Task<Department> UpdateAsync(Department department)
        {
            // Mark as modified
            _context.Departments.Update(department);
            // Set update timestamp
            department.UpdatedAt = DateTime.UtcNow;
            // Save changes
            await _context.SaveChangesAsync();
            // Return updated department
            return department;
        }

        // Delete department
        public async Task<bool> DeleteAsync(Guid id)
        {
            // Find department
            var department = await _context.Departments.FindAsync(id);
            // Return false if not found
            if (department == null) return false;
            
            // Remove from context
            _context.Departments.Remove(department);
            // Save and return result
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

