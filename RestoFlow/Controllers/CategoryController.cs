using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            var dto = categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive
            });

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Category not found", Key = "category_not_found" });
            }

            var dto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive
            };

            return Ok(dto);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                IsActive = request.IsActive
            };

            try
            {
                var created = await _service.CreateAsync(category, request.Image);

                var dto = new CategoryResponseDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    ImageUrl = created.ImageUrl,
                    IsActive = created.IsActive
                };

                return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ErrorResponseDto { Message = ex.Message, Key = "file_required" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponseDto { Message = ex.Message, Key = "invalid_file" });
            }
            catch (IOException)
            {
                return StatusCode(500, new ErrorResponseDto { Message = "Could not save file", Key = "file_save_failed" });
            }
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateDto request)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Category not found", Key = "category_not_found" });
            }

            existing.Name = request.Name;
            existing.IsActive = request.IsActive;

            try
            {
                await _service.UpdateAsync(existing, request.Image);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorResponseDto { Message = ex.Message, Key = "invalid_file" });
            }
            catch (IOException)
            {
                return StatusCode(500, new ErrorResponseDto { Message = "Could not save file", Key = "file_save_failed" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Category not found", Key = "category_not_found" });
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
