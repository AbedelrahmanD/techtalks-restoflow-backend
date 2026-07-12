using System.ComponentModel.DataAnnotations;
using RestoFlow.Enums;

namespace RestoFlow.Dtos.Requests
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }

        // Password optional for updates; leave empty to keep existing password
        [StringLength(100, MinimumLength = 6, ErrorMessage = "password_length")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Required")]
        public Role Role { get; set; }

        [EmailAddress(ErrorMessage = "InvalidEmail")]
        [StringLength(256)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "InvalidPhone")]
        [StringLength(50)]
        public string? Phone { get; set; }
    }
}
