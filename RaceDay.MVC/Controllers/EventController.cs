using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            AddApiSessionCookie(client);

            try
            {
                var details = await client.GetFromJsonAsync<EventDetailsViewModel>(
                    $"api/Events/{id}");

                if (details == null)
                    return NotFound();

                return View(details);
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to load event details. Please try again.";
                return View("Details", new EventDetailsViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(int categoryId, int eventId)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || role != "Participant")
            {
                TempData["Error"] = "Please log in as a participant to enrol.";
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            AddApiSessionCookie(client);

            try
            {
                var response = await client.PostAsync(
                    $"api/Enrolments/{categoryId}",
                    null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "You have successfully enrolled in this race category!";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    TempData["Error"] = string.IsNullOrWhiteSpace(errorMessage)
                        ? "Enrolment failed. Please try again."
                        : errorMessage;
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to connect to RaceDay. Please try again.";
            }

            return RedirectToAction("Details", new { id = eventId });
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
}