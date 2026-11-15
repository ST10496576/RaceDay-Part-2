using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ParticipantController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var events = await client.GetFromJsonAsync<List<EventViewModel>>(
                    "api/Events");

                return View(events ?? new List<EventViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to load events. Please try again later.";
                return View(new List<EventViewModel>());
            }
        }

        public async Task<IActionResult> MyEnrolments()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var enrolments =
                    await client.GetFromJsonAsync<List<EnrolmentViewModel>>(
                        "api/Enrolments/my-enrolments");

                return View(enrolments ?? new List<EnrolmentViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to load your enrolments. Please try again.";
                return View(new List<EnrolmentViewModel>());
            }
        }

        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var results =
                    await client.GetFromJsonAsync<List<ResultViewModel>>(
                        "api/Results/my-results");

                return View(results ?? new List<ResultViewModel>());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to load your results. Please try again.";
                return View(new List<ResultViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var profile = await client.GetFromJsonAsync<ProfileViewModel>(
                    "api/Profile/my-profile");

                return View(profile ?? new ProfileViewModel());
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDay.";
                return View(new ProfileViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel profile)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var response = await client.PutAsJsonAsync(
                    "api/Users/profile",
                    new
                    {
                        profile.FirstName,
                        profile.LastName,
                        profile.Email,
                        profile.PhoneNumber
                    });

                if (response.IsSuccessStatusCode)
                    ViewBag.Success = "Your profile has been updated successfully.";
                else
                    ViewBag.Error = "Profile update failed. Please check your details.";
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Unable to connect to RaceDay. Please try again.";
            }

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a picture.";
                return RedirectToAction(nameof(Profile));
            }

            using var content = new MultipartFormDataContent();

            using var fileContent = new StreamContent(file.OpenReadStream());

            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(
                    file.ContentType);

            content.Add(fileContent, "file", file.FileName);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            AddApiSessionCookie(client);

            try
            {
                var response = await client.PostAsync(
                    "api/Profile/upload-picture", content);

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Profile picture uploaded successfully.";
                else
                    TempData["Error"] = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = "Unable to connect to RaceDay.";
            }

            return RedirectToAction(nameof(Profile));
        }

        private void AddApiSessionCookie(HttpClient client)
        {
            var apiCookie = HttpContext.Session.GetString("ApiSessionCookie");

            if (!string.IsNullOrEmpty(apiCookie))
                client.DefaultRequestHeaders.Add("Cookie", apiCookie);
        }
    }
}