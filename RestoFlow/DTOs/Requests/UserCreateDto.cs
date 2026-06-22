using System.ComponentModel.DataAnnotations;
using RestoFlow.Enums;

namespace RestoFlow.Dtos.Requests
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
         public Role Role { get; set; }
    }
}
