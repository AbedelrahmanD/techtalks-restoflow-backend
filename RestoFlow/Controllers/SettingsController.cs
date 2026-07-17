using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestoFlow.Services.Interfaces;
using Microsoft.Extensions.Localization;
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

            var dto = new SettingResponseDto
            {
                Currency = settings.Currency,
                RestaurantName = settings.RestaurantName,
                LogoUrl = settings.LogoUrl,

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

                return Ok(new ApiResponseDto { 
                        Message = _localizer["saved"],
                        Data = new SettingResponseDto
                        {
                            Currency = saved.Currency,
                            RestaurantName = saved.RestaurantName,
                            LogoUrl = saved.LogoUrl,
                        }
                });

                
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new ErrorResponseDto { Message = _localizer["file_required"], Key = "file_required" });
            }
            catch (InvalidOperationException ex)
            {
                var msg = ex.Message ?? "Invalid file";

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
