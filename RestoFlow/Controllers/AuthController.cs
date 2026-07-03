using Microsoft.AspNetCore.Mvc;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Services.Interfaces;
using RestoFlow.Services;
using BCrypt.Net;
using Microsoft.Extensions.Localization;
using RestoFlow;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AuthController(IUserService userService, TokenService tokenService, IStringLocalizer<SharedResource> localizer)
        {
            _userService = userService;
            _tokenService = tokenService;
            _localizer = localizer;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto request)
        {
            var user = await _userService.GetByNameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized(new ErrorResponseDto { Message = _localizer["invalid_credentials"], Key = "invalid_credentials" });
            }

            var verified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!verified)
            {
                return Unauthorized(new ErrorResponseDto { Message = _localizer["invalid_credentials"], Key = "invalid_credentials" });
            }

            var token = _tokenService.GenerateToken(user.Id.ToString(), user.Role.ToString());

            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Role = user.Role
            };

            var response = new AuthResponseDto
            {
                Token = token,
                User = userDto
            };

            return Ok(response);
        }
    }
}
