namespace RestoFlow.Dtos.Responses
{
    public class FeedbackReportResponseDto
    {
        public int TotalSessions { get; set; }

        public double OverallAverageRating { get; set; }

        public List<FeedbackQuestionReportDto> Questions { get; set; } = new();

        public List<RecentFeedbackDto> RecentFeedback { get; set; } = new();
    }

    public class FeedbackQuestionReportDto
    {
        public int QuestionId { get; set; }

        public string Question { get; set; } = string.Empty;

        public double AverageRating { get; set; }

        public int ResponseCount { get; set; }

        public Dictionary<int, int> RatingDistribution { get; set; } = new();
    }

    public class RecentFeedbackDto
    {
        public int SessionId { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerNote { get; set; }

        public DateTime CreatedAt { get; set; }

        public double AverageRating { get; set; }
    }
}