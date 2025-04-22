using KmtBackend.API.DTOs.Department;
// Department DTOs
using KmtBackend.API.Services.Interfaces;
// Service interfaces
using KmtBackend.DAL.Entities;
// Domain models
using KmtBackend.DAL.Repositories.Interfaces;
// Repository interfaces
using MapsterMapper;
// Object mapping
using System;
// General utilities
using System.Collections.Generic;
// Collections
using System.Threading.Tasks;
// Async operations

namespace KmtBackend.API.Services
{
    // Department service implementation
    public class DepartmentService : IDepartmentService
    {
        // Data access through repository
        private readonly IDepartmentRepository _departmentRepository;
        // User repository for counting users
        private readonly IUserRepository _userRepository;
        // Object mapper
        private readonly IMapper _mapper;

        // Constructor with dependencies
        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            // Store dependencies
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // Get department by ID
        public async Task<DepartmentResponse?> GetDepartmentByIdAsync(int id)
        {
            // Find department
            var department = await _departmentRepository.GetByIdAsync(id);
            
            // Return null if not found
            if (department == null) return null;
            
            // Map to response DTO
            var response = _mapper.Map<DepartmentResponse>(department);
            
            // Get user count for department
            var users = await _userRepository.GetByDepartmentAsync(id);
            response.UserCount = users.Count();
            
            // Return response
            return response;
        }

        // Get all departments
        public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync()
        {
            // Get all departments
            var departments = await _departmentRepository.GetAllAsync();
            
            // Map to response DTOs
            var responses = _mapper.Map<IEnumerable<DepartmentResponse>>(departments).ToList();
            
            // Get all users for counting
            var allUsers = await _userRepository.GetAllAsync();
            
            // Calculate user count for each department
            foreach (var response in responses)
            {
                // Count users in this department
                response.UserCount = allUsers.Count(u => u.DepartmentId == response.Id);
            }
            
            // Return response collection
            return responses;
        }

        // Create new department
        public async Task<DepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request)
        {
            // Map request to domain model
            var department = new Department
            {
                Name = request.Name,
                NameAr = request.NameAr,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };
            
            // Create department
            var createdDepartment = await _departmentRepository.CreateAsync(department);
            
            // Map to response
            var response = _mapper.Map<DepartmentResponse>(createdDepartment);
            
            // No users in new department
            response.UserCount = 0;
            
            // Return response
            return response;
        }

        // Update existing department
        public async Task<DepartmentResponse> UpdateDepartmentAsync(int id, UpdateDepartmentRequest request)
        {
            // Find department
            var department = await _departmentRepository.GetByIdAsync(id);
            
            // Throw if not found
            if (department == null)
            {
                throw new Exception("Department not found");
            }

            // Update properties
            department.Name = request.Name;
            department.NameAr = request.NameAr;
            department.Description = request.Description;
            department.UpdatedAt = DateTime.UtcNow;
            
            // Save changes
            var updatedDepartment = await _departmentRepository.UpdateAsync(department);
            
            // Map to response
            var response = _mapper.Map<DepartmentResponse>(updatedDepartment);
            
            // Get user count
            var users = await _userRepository.GetByDepartmentAsync(id);
            response.UserCount = users.Count();
            
            // Return response
            return response;
        }

        // Delete department
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            // Get users in department
            var users = await _userRepository.GetByDepartmentAsync(id);
            
            // Don't delete if has users
            if (users.Any())
            {
                throw new Exception("Cannot delete department with assigned users");
            }
            
            // Delete and return result
            return await _departmentRepository.DeleteAsync(id);
        }
    }
}

