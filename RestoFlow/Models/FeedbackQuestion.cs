namespace RestoFlow.Models
{
    public class FeedbackQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
