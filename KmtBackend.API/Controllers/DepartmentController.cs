using KmtBackend.API.DTOs.Department;
// Department DTOs
using KmtBackend.API.Services.Interfaces;
// Service interfaces
using Microsoft.AspNetCore.Authorization;
// Authorization attributes
using Microsoft.AspNetCore.Mvc;
// MVC components
using System.Threading.Tasks;
// Async operations

namespace KmtBackend.API.Controllers
{
    // Department management controller
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        // Department service for business logic
        private readonly IDepartmentService _departmentService;

        // Constructor with DI
        public DepartmentController(IDepartmentService departmentService)
        {
            // Store service reference
            _departmentService = departmentService;
        }

        // Get all departments endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get all departments
            var departments = await _departmentService.GetAllDepartmentsAsync();
            // Return department collection
            return Ok(departments);
        }

        // Get department by ID endpoint
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Get specific department
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            
            // Return 404 if not found
            if (department == null)
                return NotFound();
                
            // Return department data
            return Ok(department);
        }

        // Create department endpoint (admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDepartmentRequest request)
        {
            try
            {
                // Create new department
                var department = await _departmentService.CreateDepartmentAsync(request);
                
                // Return created at action result
                return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
            }
            catch (Exception ex)
            {
                // Return error with message
                return BadRequest(new { message = ex.Message });
            }
        }

        // Update department endpoint (admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateDepartmentRequest request)
        {
            try
            {
                // Update existing department
                var department = await _departmentService.UpdateDepartmentAsync(id, request);
                
                // Return updated department
                return Ok(department);
            }
            catch (Exception ex)
            {
                // Handle not found
                if (ex.Message.Contains("not found"))
                    return NotFound(new { message = ex.Message });
                    
                // Handle other errors
                return BadRequest(new { message = ex.Message });
            }
        }

        // Delete department endpoint (admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            // Attempt deletion
            var result = await _departmentService.DeleteDepartmentAsync(id);
            
            // Return 404 if department not found
            if (!result)
                return NotFound();
                
            // Return success with no content
            return NoContent();
        }
    }
}

