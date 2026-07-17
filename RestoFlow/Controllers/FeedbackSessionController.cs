using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RestoFlow.Dtos.Requests;
using RestoFlow.Dtos.Responses;
using RestoFlow.Models;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackSessionController : ControllerBase
    {
        private readonly IFeedbackSessionService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public FeedbackSessionController(
            IFeedbackSessionService service,
            IStringLocalizer<SharedResource> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(
            [FromBody] FeedbackSessionCreateDto request)
        {
            var questionIds = request.Responses
                .Select(response => response.QuestionId)
                .ToList();

            if (questionIds.Count != questionIds.Distinct().Count())
            {
                return BadRequest(new ErrorResponseDto
                {
                    Message = _localizer["feedback_question_duplicate"],
                    Key = "feedback_question_duplicate"
                });
            }

            if (!await _service.QuestionsExistAsync(questionIds))
            {
                return BadRequest(new ErrorResponseDto
                {
                    Message = _localizer["feedback_question_invalid"],
                    Key = "feedback_question_invalid"
                });
            }

            if (!await _service.QuestionsAreActiveAsync(questionIds))
            {
                return BadRequest(new ErrorResponseDto
                {
                    Message = _localizer["feedback_question_inactive"],
                    Key = "feedback_question_inactive"
                });
            }

            var session = new FeedbackSession
            {
                CustomerPhone = request.CustomerPhone?.Trim(),
                CustomerNote = request.CustomerNote?.Trim(),
                Responses = request.Responses.Select(response =>
                    new FeedbackResponse
                    {
                        QuestionId = response.QuestionId,
                        Rating = response.Rating
                    }).ToList()
            };

            var created = await _service.CreateAsync(session);

            return Ok(new
            {
                id = created.Id,
                message = "Feedback submitted successfully"
            });
        }
    }
}