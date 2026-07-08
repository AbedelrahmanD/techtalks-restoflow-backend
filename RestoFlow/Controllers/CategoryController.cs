using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using Microsoft.Extensions.Localization;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoryController(ICategoryService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
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
                return NotFound(new ErrorResponseDto { Message = _localizer["category_not_found"], Key = "category_not_found" });
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
            var existingByName = await _service.GetByNameAsync(request.Name);
            if (existingByName != null)
            {
                return Conflict(new ErrorResponseDto { Message = _localizer["category_name_taken"], Key = "category_name_taken" });
            }

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
                IsActive = created.IsActive,
                Message = _localizer["category_created"],
                Key = "category_created"
            };

            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateDto request)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = _localizer["category_not_found"], Key = "category_not_found" });
            }

            var existingByName = await _service.GetByNameAsync(request.Name);
            if (existingByName != null && existingByName.Id != id)
            {
                return Conflict(new ErrorResponseDto { Message = _localizer["category_name_taken"], Key = "category_name_taken" });
            }

            existing.Name = request.Name;
            existing.IsActive = request.IsActive;

            await _service.UpdateAsync(existing, request.Image);

            var dto = new CategoryResponseDto
            {
                Id = existing.Id,
                Name = existing.Name,
                ImageUrl = existing.ImageUrl,
                IsActive = existing.IsActive,
                Message = _localizer["category_updated"],
                Key = "category_updated"
            };

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = _localizer["category_not_found"], Key = "category_not_found" });
            }

            await _service.DeleteAsync(id);
            return Ok(new SuccessResponseDto { Message = _localizer["category_deleted"], Key = "category_deleted" });
        }
    }
}
