using Microsoft.EntityFrameworkCore;
using RestoFlow.Data;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Services.Implementations
{
    public class FeedbackQuestionService : IFeedbackQuestionService
    {
        private readonly AppDbContext _db;

        public FeedbackQuestionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FeedbackQuestion>> GetAllAsync()
        {
            return await _db.FeedbackQuestions.AsNoTracking().ToListAsync();
        }

        public async Task<FeedbackQuestion?> GetByIdAsync(int id)
        {
            return await _db.FeedbackQuestions.FindAsync(id);
        }

        public async Task<FeedbackQuestion> CreateAsync(FeedbackQuestion question)
        {
            _db.FeedbackQuestions.Add(question);
            await _db.SaveChangesAsync();
            return question;
        }

        public async Task UpdateAsync(FeedbackQuestion question)
        {
            _db.FeedbackQuestions.Update(question);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _db.FeedbackQuestions.FindAsync(id);
            if (existing == null)
            {
                return;
            }

            _db.FeedbackQuestions.Remove(existing);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> ExistsByQuestionAsync(string question, int? excludeId = null)
        {
            var normalizedQuestion = question.Trim().ToLower();

            return await _db.FeedbackQuestions.AnyAsync(q =>
                q.Question.Trim().ToLower() == normalizedQuestion &&
                (!excludeId.HasValue || q.Id != excludeId.Value));
        }
    }
}