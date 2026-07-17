namespace RestoFlow.Dtos.Responses
{
    public class FeedbackResponseResponseDto
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string Question { get; set; } = string.Empty;

        public int Rating { get; set; }
    }
}