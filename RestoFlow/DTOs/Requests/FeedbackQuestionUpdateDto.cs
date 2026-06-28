using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class FeedbackQuestionUpdateDto
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Question { get; set; }

        public bool IsActive { get; set; } = true;
    }
}