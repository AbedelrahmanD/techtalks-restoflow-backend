namespace RestoFlow.Dtos.Responses
{
    public class FeedbackSessionResponseDto
    {
        public int Id { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerNote { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<FeedbackResponseResponseDto> Responses { get; set; } = new();
    }
}