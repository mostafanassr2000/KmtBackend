using KmtBackend.API.DTOs.Department;
using KmtBackend.BLL.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KmtBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentManager _departmentService;

        public DepartmentController(IDepartmentManager departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            
            if (department == null)
                return NotFound();
                
            return Ok(department);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDepartmentRequest request)
        {
            try
            {
                var department = await _departmentService.CreateDepartmentAsync(request);
                
                return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateDepartmentRequest request)
        {
            try
            {
                var department = await _departmentService.UpdateDepartmentAsync(id, request);
                
                return Ok(department);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new { message = ex.Message });
                    
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
    }
}

