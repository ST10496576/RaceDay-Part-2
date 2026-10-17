using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public ResultsController(RaceDayContext context)
        {
            _context = context;
        }

        // View results for the logged-in participant
        [HttpGet("my-results")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role == null)
                return Unauthorized(new { message = "Please log in first." });

            if (role != "Participant")
                return StatusCode(403, new { message = "Participants only." });

            var results = await _context.Results
                .Join(
                    _context.Enrolments,
                    result => result.EnrolmentID,
                    enrolment => enrolment.EnrolmentID,
                    (result, enrolment) => new { result, enrolment })
                .Where(x => x.enrolment.ParticipantID == userId.Value)
                .Select(x => x.result)
                .ToListAsync();

            return Ok(results);
        }
    }
}