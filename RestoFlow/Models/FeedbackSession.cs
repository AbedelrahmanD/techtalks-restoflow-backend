namespace RestoFlow.Models
{
    public class FeedbackSession
    {
        public int Id { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerNote { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<FeedbackResponse>? Responses { get; set; }
    }
}
