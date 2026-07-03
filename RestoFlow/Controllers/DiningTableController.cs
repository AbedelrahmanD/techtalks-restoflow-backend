using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;
using Microsoft.Extensions.Localization;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DiningTableController : ControllerBase
    {
        private readonly IDiningTableService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DiningTableController(IDiningTableService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tables = await _service.GetAllAsync();
            var dto = tables.Select(t => new DiningTableResponseDto
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                SeatingCapacity = t.SeatingCapacity,
                QrCodeToken = t.QrCodeToken
            });
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var table = await _service.GetByIdAsync(id);
            if (table == null)
                return NotFound(new ErrorResponseDto { Message = _localizer["table_not_found"], Key = "table_not_found" });

            return Ok(new DiningTableResponseDto
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                SeatingCapacity = table.SeatingCapacity,
                QrCodeToken = table.QrCodeToken
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiningTableCreateDto request)
        {
            var table = new DiningTable
            {
                TableNumber = request.TableNumber,
                SeatingCapacity = request.SeatingCapacity
                
            };

            var created = await _service.CreateAsync(table);

            var dto = new DiningTableResponseDto
            {
                Id = created.Id,
                TableNumber = created.TableNumber,
                SeatingCapacity = created.SeatingCapacity,
                QrCodeToken = created.QrCodeToken
            };

            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiningTableUpdateDto request)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new ErrorResponseDto { Message = _localizer["table_not_found"], Key = "table_not_found" });

            existing.TableNumber = request.TableNumber;
            existing.SeatingCapacity = request.SeatingCapacity;
            

            await _service.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new ErrorResponseDto { Message = _localizer["table_not_found"], Key = "table_not_found" });

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
