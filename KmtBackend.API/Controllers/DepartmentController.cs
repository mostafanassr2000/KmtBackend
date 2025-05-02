using KmtBackend.API.Attributes;
using KmtBackend.API.Common;
using KmtBackend.API.DTOs.Department;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Constants;
using KmtBackend.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [RequirePermission(Permissions.ViewDepartments)]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(new ResponseWrapper(departments, "Retrieved Departments Successfully.", true));
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.ViewDepartments)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            
            if (department == null)
                return NotFound(new ResponseWrapper(null, "Department Not Found", false));

            return Ok(new ResponseWrapper(department, "Retrieved Department Successfully.", true));
        }

        [HttpPost]
        [RequirePermission(Permissions.CreateDepartments)]
        public async Task<IActionResult> Create(CreateDepartmentRequest request)
        {
            try
            {
                var department = await _departmentService.CreateDepartmentAsync(request);
                return CreatedAtAction(null, new ResponseWrapper(department, "Department Created Succesfully.", true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.UpdateDepartments)]
        public async Task<IActionResult> Update(Guid id, UpdateDepartmentRequest request)
        {
            try
            {
                var department = await _departmentService.UpdateDepartmentAsync(id, request);

                return Ok(new ResponseWrapper(department, "Department Updated Successfully.", true));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new ResponseWrapper(null, "Department Not Found", false));

                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.DeleteDepartments)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            
            if (!result)
                return NotFound(new ResponseWrapper(null, "Department Not Found", false));

            return NoContent();
        }
    }
}

