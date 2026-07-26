using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;
using RestoFlow.Dtos.Responses;
namespace RestoFlow.Services.Implementations
{
    public class FeedbackSessionService : IFeedbackSessionService
    {
        private readonly AppDbContext _db;

        public FeedbackSessionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FeedbackSession>> GetAllAsync()
        {
            return await _db.FeedbackSessions
                .AsNoTracking()
                .Include(session => session.Responses)
                    .ThenInclude(response => response.Question)
                .OrderByDescending(session => session.CreatedAt)
                .ToListAsync();
        }

        public async Task<FeedbackSession?> GetByIdAsync(int id)
        {
            return await _db.FeedbackSessions
                .AsNoTracking()
                .Include(session => session.Responses)
                    .ThenInclude(response => response.Question)
                .FirstOrDefaultAsync(session => session.Id == id);
        }

        public async Task<FeedbackSession> CreateAsync(FeedbackSession session)
        {
            _db.FeedbackSessions.Add(session);
            await _db.SaveChangesAsync();

            return session;
        }

        public async Task<bool> QuestionsExistAsync(IEnumerable<int> questionIds)
        {
            var ids = questionIds.Distinct().ToList();

            var existingCount = await _db.FeedbackQuestions
                .CountAsync(question => ids.Contains(question.Id));

            return existingCount == ids.Count;
        }

        public async Task<bool> QuestionsAreActiveAsync(IEnumerable<int> questionIds)
        {
            var ids = questionIds.Distinct().ToList();

            var activeCount = await _db.FeedbackQuestions
                .CountAsync(question =>
                    ids.Contains(question.Id) &&
                    question.IsActive);

            return activeCount == ids.Count;
        }
        public async Task<FeedbackReportResponseDto> GetReportAsync()
        {
            var sessions = await _db.FeedbackSessions
                .AsNoTracking()
                .Include(session => session.Responses)
                    .ThenInclude(response => response.Question)
                .OrderByDescending(session => session.CreatedAt)
                .ToListAsync();

            var allResponses = sessions
                .SelectMany(session => session.Responses ?? Enumerable.Empty<FeedbackResponse>())
                .ToList();

            var questionReports = allResponses
                .Where(response => response.Question != null)
                .GroupBy(response => new
                {
                    response.QuestionId,
                    Question = response.Question!.Question
                })
                .Select(group => new FeedbackQuestionReportDto
                {
                    QuestionId = group.Key.QuestionId,
                    Question = group.Key.Question,
                    AverageRating = Math.Round(group.Average(x => x.Rating), 2),
                    ResponseCount = group.Count(),
                    RatingDistribution = Enumerable.Range(1, 5)
                        .ToDictionary(
                            rating => rating,
                            rating => group.Count(x => x.Rating == rating)
                        )
                })
                .OrderBy(question => question.QuestionId)
                .ToList();

            var recentFeedback = sessions
                .Take(10)
                .Select(session => new RecentFeedbackDto
                {
                    SessionId = session.Id,
                    CustomerPhone = session.CustomerPhone,
                    CustomerNote = session.CustomerNote,
                    CreatedAt = session.CreatedAt,
                    AverageRating =
                        session.Responses != null && session.Responses.Any()
                            ? Math.Round(session.Responses.Average(x => x.Rating), 2)
                            : 0
                })
                .ToList();

            return new FeedbackReportResponseDto
            {
                TotalSessions = sessions.Count,
                OverallAverageRating = allResponses.Any()
                    ? Math.Round(allResponses.Average(x => x.Rating), 2)
                    : 0,
                Questions = questionReports,
                RecentFeedback = recentFeedback
            };
        }
    }
}