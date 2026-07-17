using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoFlow.Dtos.Responses;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/customer-feedback")]
    [AllowAnonymous]
    public class CustomerFeedbackController : ControllerBase
    {
        private readonly IFeedbackQuestionService _questionService;

        public CustomerFeedbackController(
            IFeedbackQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetActiveQuestions()
        {
            var questions = await _questionService.GetAllAsync();

            var dto = questions
                .Where(q => q.IsActive)
                .Select(q => new FeedbackQuestionResponseDto
                {
                    Id = q.Id,
                    Question = q.Question,
                    IsActive = q.IsActive
                });

            return Ok(dto);
        }
    }
}