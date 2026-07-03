using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class AuthRequestDto
    {
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}
