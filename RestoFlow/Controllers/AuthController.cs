using Microsoft.AspNetCore.Mvc;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Services.Interfaces;
using RestoFlow.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly IRefreshTokenService _refreshService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _config;

        private const string RefreshCookieName = "refreshToken";
        private const string AccessCookieName = "accessToken";

        public AuthController(IUserService userService, TokenService tokenService, IRefreshTokenService refreshService, IStringLocalizer<SharedResource> localizer, IConfiguration config)
        {
            _userService = userService;
            _tokenService = tokenService;
            _refreshService = refreshService;
            _localizer = localizer;
            _config = config;
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

            // create refresh token and set as secure HttpOnly cookie
            var rt = await _refreshService.CreateAsync(user.Id);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = rt.Expires
            };
            Response.Cookies.Append(RefreshCookieName, rt.Token, cookieOptions);

            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpirationInMinutes"]!))
            };
            Response.Cookies.Append(AccessCookieName, token, accessCookieOptions);

            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Role = new RoleResponseDto
                {
                    id = (int)user.Role,
                    role = user.Role.ToString()
                },
                Email = user.Email,
                Phone = user.Phone
            };

            var response = new AuthResponseDto
            {
                User = userDto
            };

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue(RefreshCookieName, out var existingToken) || string.IsNullOrEmpty(existingToken))
            {
                return Unauthorized(new ErrorResponseDto { Message = _localizer["refresh_missing"], Key = "refresh_missing" });
            }

            var rt = await _refreshService.GetByTokenAsync(existingToken);
            if (rt == null || rt.Revoked || rt.Expires < DateTime.UtcNow)
            {
                return Unauthorized(new ErrorResponseDto { Message = _localizer["invalid_refresh"], Key = "invalid_refresh" });
            }

            var user = rt.User;
            if (user == null)
            {
                return Unauthorized(new ErrorResponseDto { Message = _localizer["invalid_refresh"], Key = "invalid_refresh" });
            }

            // rotate refresh token: revoke old and issue new
            var newRt = await _refreshService.CreateAsync(user.Id);
            await _refreshService.RevokeAsync(rt, newRt.Token);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = newRt.Expires
            };
            Response.Cookies.Append(RefreshCookieName, newRt.Token, cookieOptions);

            var access = _tokenService.GenerateToken(user.Id.ToString(), user.Role.ToString());


            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpirationInMinutes"]!))
            };
            Response.Cookies.Append(AccessCookieName, access, accessCookieOptions);

            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Role = new RoleResponseDto
                {
                    id = (int)user.Role,
                    role = user.Role.ToString()
                },

                Email = user.Email,
                Phone = user.Phone
            };

            return Ok(new AuthResponseDto { User = userDto });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue(RefreshCookieName, out var existingToken) && !string.IsNullOrEmpty(existingToken))
            {
                var rt = await _refreshService.GetByTokenAsync(existingToken);
                if (rt != null)
                {
                    await _refreshService.RevokeAsync(rt);
                }
            }

            Response.Cookies.Delete(RefreshCookieName);
            Response.Cookies.Delete(AccessCookieName);
            return NoContent();
        }
    }
}
