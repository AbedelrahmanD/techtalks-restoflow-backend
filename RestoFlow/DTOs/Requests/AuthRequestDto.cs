using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class AuthRequestDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
