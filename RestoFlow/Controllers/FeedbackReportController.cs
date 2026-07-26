using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoFlow.Services.Interfaces;

namespace RestoFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class FeedbackReportController : ControllerBase
    {
        private readonly IFeedbackSessionService _service;

        public FeedbackReportController(IFeedbackSessionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport()
        {
            var report = await _service.GetReportAsync();

            return Ok(report);
        }
    }
}