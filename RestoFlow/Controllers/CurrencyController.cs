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
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CurrencyController(ICurrencyService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await _service.GetAllAsync();
            var dto = currencies.Select(c => new CurrencyResponseDto
            {
                Id = c.Id,
                Code = c.Code,
                Symbol = c.Symbol,
                Name = c.Name
            });

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var currency = await _service.GetByIdAsync(id);
            if (currency == null)
            {
                return NotFound(new ErrorResponseDto { Message = _localizer["currency_not_found"], Key = "currency_not_found" });
            }

            var dto = new CurrencyResponseDto
            {
                Id = currency.Id,
                Code = currency.Code,
                Symbol = currency.Symbol,
                Name = currency.Name
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurrencyCreateDto request)
        {
            var existing = await _service.GetByCodeAsync(request.Code);
            if (existing != null)
            {
                return Conflict(new ErrorResponseDto { Message = _localizer["currency_code_taken"], Key = "currency_code_taken" });
            }

            var currency = new Currency
            {
                Code = request.Code,
                Symbol = request.Symbol,
                Name = request.Name
            };

            var created = await _service.CreateAsync(currency);

            var dto = new CurrencyResponseDto
            {
                Id = created.Id,
                Code = created.Code,
                Symbol = created.Symbol,
                Name = created.Name
            };

            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CurrencyUpdateDto request)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = _localizer["currency_not_found"], Key = "currency_not_found" });
            }

            var byCode = await _service.GetByCodeAsync(request.Code);
            if (byCode != null && byCode.Id != id)
            {
                return Conflict(new ErrorResponseDto { Message = _localizer["currency_code_taken"], Key = "currency_code_taken" });
            }

            existing.Code = request.Code;
            existing.Symbol = request.Symbol;
            existing.Name = request.Name;

            await _service.UpdateAsync(existing);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = _localizer["currency_not_found"], Key = "currency_not_found" });
            }

            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
