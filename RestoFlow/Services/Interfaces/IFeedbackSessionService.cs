using RestoFlow.Models;
using RestoFlow.Dtos.Responses;
namespace RestoFlow.Services.Interfaces
{
    public interface IFeedbackSessionService
    {
        Task<IEnumerable<FeedbackSession>> GetAllAsync();

        Task<FeedbackSession?> GetByIdAsync(int id);

        Task<FeedbackSession> CreateAsync(FeedbackSession session);

        Task<bool> QuestionsExistAsync(IEnumerable<int> questionIds);

        Task<bool> QuestionsAreActiveAsync(IEnumerable<int> questionIds);
        Task<FeedbackReportResponseDto> GetReportAsync();
    }
}