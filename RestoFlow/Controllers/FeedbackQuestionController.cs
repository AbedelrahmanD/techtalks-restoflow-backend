using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class FeedbackQuestionController : ControllerBase
    {
        private readonly IFeedbackQuestionService _service;

        public FeedbackQuestionController(IFeedbackQuestionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _service.GetAllAsync();
            var dto = questions.Select(q => new FeedbackQuestionResponseDto
            {
                Id = q.Id,
                Question = q.Question,
                IsActive = q.IsActive
            });

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var question = await _service.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Feedback question not found", Key = "feedback_question_not_found" });
            }

            var dto = new FeedbackQuestionResponseDto
            {
                Id = question.Id,
                Question = question.Question,
                IsActive = question.IsActive
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedbackQuestionCreateDto request)
        {
            var question = new FeedbackQuestion
            {
                Question = request.Question,
                IsActive = request.IsActive
            };

            var created = await _service.CreateAsync(question);

            var dto = new FeedbackQuestionResponseDto
            {
                Id = created.Id,
                Question = created.Question,
                IsActive = created.IsActive
            };

            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FeedbackQuestionUpdateDto request)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Feedback question not found", Key = "feedback_question_not_found" });
            }

            existing.Question = request.Question;
            existing.IsActive = request.IsActive;

            await _service.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ErrorResponseDto { Message = "Feedback question not found", Key = "feedback_question_not_found" });
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}