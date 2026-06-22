using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestoFlow.Services.Interfaces;
using RestoFlow.Helpers;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Models;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  // [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _service;

        public SettingsController(ISettingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var settings = await _service.GetAsync();
            if (settings == null)
            {
                return NotFound(new { Message = "Settings not found", Key = "settings_not_found" });
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
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ErrorResponseDto { Message = ex.Message, Key = "file_required" });
            }
            catch (InvalidOperationException ex)
            {
                var msg = ex.Message ?? "Invalid file";
                

                return BadRequest(new ErrorResponseDto { Message = msg, Key = "invalid_file" });
            }
            catch (IOException)
            {
                return StatusCode(500, new ErrorResponseDto { Message = "Could not save file", Key = "file_save_failed" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponseDto { Message = "Unexpected error", Key = "server_error" });
            }
        }
    }
}
