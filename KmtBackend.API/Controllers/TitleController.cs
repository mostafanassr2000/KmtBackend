using KmtBackend.API.Attributes;
using KmtBackend.API.Common;
using KmtBackend.API.DTOs.Department;
using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Constants;
using KmtBackend.Models.DTOs.Title;
using Microsoft.AspNetCore.Mvc;

namespace KmtBackend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TitleController : ControllerBase
    {
        private readonly ITitleManager _titleService;

        public TitleController(ITitleManager titleService)
        {
            _titleService = titleService;
        }

        [HttpGet]
        [RequirePermission(Permissions.ViewTitles)]
        public async Task<IActionResult> GetAll()
        {
            var titles = await _titleService.GetAllTitlesAsync();
            return Ok(new ResponseWrapper(titles, "Retrieved Titles Successfully.", true));
        }

        [HttpGet("{id}")]
        [RequirePermission(Permissions.ViewTitles)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var title = await _titleService.GetTitleByIdAsync(id);
            
            if (title == null)
                return NotFound(new ResponseWrapper(null, "Title Not Found", false));

            return Ok(new ResponseWrapper(title, "Retrieved Title Successfully.", true));
        }

        [HttpPost]
        [RequirePermission(Permissions.CreateTitles)]
        public async Task<IActionResult> Create(CreateTitleRequest request)
        {
            try
            {
                var title = await _titleService.CreateTitleAsync(request);
                return CreatedAtAction(null, new ResponseWrapper(title, "Title Created Succesfully.", true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }

        [HttpPut("{id}")]
        [RequirePermission(Permissions.UpdateTitles)]
        public async Task<IActionResult> Update(Guid id, UpdateTitleRequest request)
        {
            try
            {
                var title = await _titleService.UpdateTitleAsync(id, request);

                return Ok(new ResponseWrapper(title, "Title Updated Successfully.", true));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new ResponseWrapper(null, "Title Not Found", false));

                return BadRequest(new ResponseWrapper(null, "Bad Request", false, [ex.Message]));
            }
        }

        [HttpDelete("{id}")]
        [RequirePermission(Permissions.DeleteTitles)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _titleService.DeleteTitleAsync(id);
            
            if (!result)
                return NotFound(new ResponseWrapper(null, "Title Not Found", false));

            return NoContent();
        }
    }
}

