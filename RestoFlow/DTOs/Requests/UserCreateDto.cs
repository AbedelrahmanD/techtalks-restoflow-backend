using System.ComponentModel.DataAnnotations;
using RestoFlow.Enums;

namespace RestoFlow.Dtos.Requests
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "InvalidEmail")]
        [StringLength(256)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "InvalidPhone")]
        [StringLength(50)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Required")]
         public Role Role { get; set; }
    }
}
