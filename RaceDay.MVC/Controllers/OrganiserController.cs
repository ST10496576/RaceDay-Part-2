using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganiserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var events = await client.GetFromJsonAsync<List<EventViewModel>>(
                    "api/Events/my-events");

                return View(events ?? new List<EventViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to load your events. Please try again.";
                return View(new List<EventViewModel>());
            }
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(EventViewModel newEvent)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(newEvent);
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var response = await client.PostAsJsonAsync(
                    "api/Events",
                    new
                    {
                        newEvent.EventName,
                        newEvent.Description,
                        newEvent.Location,
                        newEvent.Province,
                        newEvent.RaceDate,
                        newEvent.RegistrationDeadline
                    });

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Dashboard");
                }

                ViewBag.Error = "Unable to create the event. Please check your details.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDay. Please try again.";
            }

            return View(newEvent);
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var response = await client.GetAsync($"api/Events/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Dashboard");
                }

                var eventData = await response.Content.ReadFromJsonAsync<EventDetailsResponse>();

                if (eventData?.Event == null)
                {
                    return RedirectToAction("Dashboard");
                }

                return View(eventData.Event);
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to load the event.";
                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(int id, EventViewModel updatedEvent)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(updatedEvent);
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var response = await client.PutAsJsonAsync(
                    $"api/Events/{id}",
                    new
                    {
                        updatedEvent.EventName,
                        updatedEvent.Description,
                        updatedEvent.Location,
                        updatedEvent.Province,
                        updatedEvent.RaceDate,
                        updatedEvent.RegistrationDeadline
                    });

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Dashboard");
                }

                ViewBag.Error = "Unable to update the event. Please check your details.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDay.";
            }

            return View(updatedEvent);
        }

        // Display participants enrolled in an event
        [HttpGet]
        public async Task<IActionResult> ManageResults(int eventId)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var response = await client.GetAsync(
                    $"api/Results/event/{eventId}/enrolments");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Unable to load event participants.";
                    return RedirectToAction("Dashboard");
                }

                var participants =
                    await response.Content.ReadFromJsonAsync<List<EnrolmentResultViewModel>>();

                ViewBag.EventID = eventId;

                return View(participants ?? new List<EnrolmentResultViewModel>());
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to connect to RaceDay.";
                return RedirectToAction("Dashboard");
            }
        }

        // Save a participant's race result
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveResult(
            int eventId,
            int enrolmentId,
            TimeSpan? finishTime,
            int? position,
            decimal? averagePace)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var response = await client.PostAsJsonAsync(
                    $"api/Results/event/{eventId}/enrolments/{enrolmentId}",
                    new
                    {
                        FinishTime = finishTime,
                        Position = position,
                        AveragePace = averagePace
                    });

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Result saved successfully.";
                }
                else
                {
                    TempData["Error"] = "Unable to save the result. Please check the details.";
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to connect to RaceDay.";
            }

            return RedirectToAction("ManageResults", new { eventId });
        }

        private void AddApiSessionCookie(HttpClient client)
        {
            var apiCookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
            }
        }
    }

    public class EventDetailsResponse
    {
        public EventViewModel? Event { get; set; }
    }
}