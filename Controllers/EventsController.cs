using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventsController(RaceDayContext context)
        {
            _context = context;
        }

        // View all events
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // Create an event - organisers only
        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event newEvent)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role == null)
                return Unauthorized(new { message = "Please log in first." });

            if (role != "Organiser")
                return StatusCode(403, new { message = "Only organisers can create events." });

            newEvent.OrganiserID = userId.Value;

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Event created successfully.",
                eventId = newEvent.EventID
            });
        }
    }
}