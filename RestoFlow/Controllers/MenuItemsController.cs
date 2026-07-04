using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _service;

        public MenuItemsController(IMenuItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            var dto = items.Select(m => new MenuItemDto
            {
                Id = m.Id,
                CategoryId = m.CategoryId,
                CategoryName = m.Category?.Name,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsActive = m.IsActive
            });

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Menu item not found", Key = "menuitem_not_found" });
            }

            var dto = new MenuItemDto
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                CategoryName = item.Category?.Name,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                IsActive = item.IsActive
            };

            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateMenuItemDto request)
        {
            var menuItem = new MenuItem
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                IsActive = request.IsActive
            };

            var created = await _service.CreateAsync(menuItem, request.Image);

            var dto = new MenuItemDto
            {
                Id = created.Id,
                CategoryId = created.CategoryId,
                CategoryName = created.Category?.Name,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                ImageUrl = created.ImageUrl,
                IsActive = created.IsActive
            };

            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateMenuItemDto request)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Menu item not found", Key = "menuitem_not_found" });
            }

            existing.CategoryId = request.CategoryId;
            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.Price = request.Price;
            existing.IsActive = request.IsActive;

            await _service.UpdateAsync(existing, request.Image);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Menu item not found", Key = "menuitem_not_found" });
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
