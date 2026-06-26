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
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                IsActive = request.IsActive
            };

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateDto request)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Category not found", Key = "category_not_found" });
            }

            existing.Name = request.Name;
            existing.IsActive = request.IsActive;

            await _service.UpdateAsync(existing, request.Image);
            return NoContent();
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
