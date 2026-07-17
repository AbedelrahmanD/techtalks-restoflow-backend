using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class FeedbackSessionCreateDto
    {
        [Phone(ErrorMessage = "InvalidPhone")]
        [StringLength(30)]
        public string? CustomerPhone { get; set; }

        [StringLength(1000)]
        public string? CustomerNote { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(1)]
        public List<FeedbackResponseCreateDto> Responses { get; set; } = new();
    }
}