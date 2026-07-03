using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using RestoFlow.Services.Interfaces;
using RestoFlow.Helpers;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Models;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SettingsController(ISettingService service, IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var settings = await _service.GetAsync();
            if (settings == null)
            {
                return NotFound(new { Message = _localizer["settings_not_found"], Key = "settings_not_found" });
            }

            var dto = new SettingResponseDto
            {
                Id = settings.Id,
                CurrencyId = settings.CurrencyId,
                RestaurantName = settings.RestaurantName,
                LogoUrl = settings.LogoUrl,
                UpdatedAt = settings.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Save([FromForm] SettingRequestDto request)
        {
            var model = new Setting
            {
                CurrencyId = request.CurrencyId,
                RestaurantName = request.RestaurantName
            };

            try
            {
                var saved = await _service.SaveAsync(model, request.Logo);

                var dto = new SettingResponseDto
                {
                    Id = saved.Id,
                    CurrencyId = saved.CurrencyId,
                    RestaurantName = saved.RestaurantName,
                    LogoUrl = saved.LogoUrl,
                    UpdatedAt = saved.UpdatedAt
                };

                return Ok(dto);
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new ErrorResponseDto { Message = _localizer["file_required"], Key = "file_required" });
            }
            catch (InvalidOperationException ex)
            {
                var msg = ex.Message ?? _localizer["invalid_file"];
                return BadRequest(new ErrorResponseDto { Message = msg, Key = "invalid_file" });
            }
            catch (IOException)
            {
                return StatusCode(500, new ErrorResponseDto { Message = _localizer["file_save_failed"], Key = "file_save_failed" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponseDto { Message = _localizer["server_error"], Key = "server_error" });
            }
        }
    }
}
