namespace RestoFlow.Models
{
    public class FeedbackResponse
    {
        public int Id { get; set; }
        public int FeedbackSessionId { get; set; }
        public FeedbackSession? FeedbackSession { get; set; }
        public int QuestionId { get; set; }
        public FeedbackQuestion? Question { get; set; }
        public int Rating { get; set; }
    }
}
