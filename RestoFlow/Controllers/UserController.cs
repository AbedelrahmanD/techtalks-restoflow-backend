using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            var dto = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                Role = u.Role,
                Email = u.Email,
                Phone = u.Phone
            });

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ErrorResponseDto { Message = "User not found", Key = "user_not_found" });
            }
            var dto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Role = user.Role,
                Email = user.Email,
                Phone = user.Phone
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto request)
        {
            // check unique username
            var existing = await _service.GetByNameAsync(request.Username);
            if (existing != null)
            {
                return Conflict(new ErrorResponseDto { Message = "Username already exists", Key = "username_taken" });
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var existingEmail = await _service.GetByEmailAsync(request.Email!);
                if (existingEmail != null)
                {
                    return Conflict(new ErrorResponseDto { Message = "Email already exists", Key = "email_taken" });
                }
            }

            var user = new User
            {
                Username = request.Username,
                Role = request.Role
                ,
                Email = request.Email,
                Phone = request.Phone
            };

            var created = await _service.CreateAsync(user, request.Password);

            var dto = new UserResponseDto
            {
                Id = created.Id,
                Username = created.Username,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt,
                Role = created.Role,
                Email = created.Email,
                Phone = created.Phone
            };

            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto request)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "User not found", Key = "user_not_found" });
            }

            // if username changed ensure uniqueness
            if (!string.Equals(existing.Username, request.Username, StringComparison.OrdinalIgnoreCase))
            {
                var other = await _service.GetByNameAsync(request.Username);
                if (other != null && other.Id != id)
                {
                    return Conflict(new ErrorResponseDto { Message = "Username already exists", Key = "username_taken" });
                }
            }

            // if email provided and changed ensure uniqueness
            var newEmail = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email;
            if (!string.Equals(existing.Email, newEmail, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(newEmail))
                {
                    var otherByEmail = await _service.GetByEmailAsync(newEmail!);
                    if (otherByEmail != null && otherByEmail.Id != id)
                    {
                        return Conflict(new ErrorResponseDto { Message = "Email already exists", Key = "email_taken" });
                    }
                }
            }

            existing.Username = request.Username;
            existing.Role = request.Role;
            existing.Email = newEmail;
            existing.Phone = request.Phone;

            var plainPassword = string.IsNullOrWhiteSpace(request.Password) ? null : request.Password;

            await _service.UpdateAsync(existing, plainPassword);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "User not found", Key = "user_not_found" });
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
