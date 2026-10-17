using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EnrolmentsController(RaceDayContext context)
        {
            _context = context;
        }

        // View enrolments for the logged-in participant
        [HttpGet("my-enrolments")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role == null)
                return Unauthorized(new { message = "Please log in first." });

            if (role != "Participant")
                return StatusCode(403, new { message = "Participants only." });

            var enrolments = await _context.Enrolments
                .Where(e => e.ParticipantID == userId.Value)
                .ToListAsync();

            return Ok(enrolments);
        }

        // Enrol in a category
        [HttpPost("{categoryId:int}")]
        public async Task<IActionResult> Enrol(int categoryId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role == null)
                return Unauthorized(new { message = "Please log in first." });

            if (role != "Participant")
                return StatusCode(403, new { message = "Participants only." });

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == categoryId);

            if (category == null)
                return NotFound(new { message = "Category not found." });

            var enrolment = new Enrolment
            {
                ParticipantID = userId.Value,
                CategoryID = categoryId,
                RegistrationDaate = DateTime.UtcNow
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Enrolment successful.",
                enrolmentId = enrolment.EnrolmentID
            });
        }
    }
}