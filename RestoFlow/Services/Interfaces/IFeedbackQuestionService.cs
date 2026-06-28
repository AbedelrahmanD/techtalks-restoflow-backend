using RestoFlow.Models;

namespace RestoFlow.Services.Interfaces
{
    public interface IFeedbackQuestionService
    {
        Task<IEnumerable<FeedbackQuestion>> GetAllAsync();
        Task<FeedbackQuestion?> GetByIdAsync(int id);
        Task<FeedbackQuestion> CreateAsync(FeedbackQuestion question);
        Task UpdateAsync(FeedbackQuestion question);
        Task DeleteAsync(int id);
    }
}