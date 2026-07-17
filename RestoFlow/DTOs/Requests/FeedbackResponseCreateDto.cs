using System.ComponentModel.DataAnnotations;

namespace RestoFlow.Dtos.Requests
{
    public class FeedbackResponseCreateDto
    {
        [Range(1, int.MaxValue)]
        public int QuestionId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
    }
}