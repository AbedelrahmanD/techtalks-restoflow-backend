using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

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
    }
}