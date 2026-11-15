using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/Auth/login",
                new { Email = email, Password = password });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            var user = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (user == null)
            {
                ViewBag.Error = "Unable to process login.";
                return View();
            }

            HttpContext.Session.SetInt32("UserID", user.UserId);
            HttpContext.Session.SetString("Role", user.Role);
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var sessionCookie = cookies.FirstOrDefault(
                    c => c.StartsWith(".AspNetCore.Session=",
                    StringComparison.OrdinalIgnoreCase));

                if (sessionCookie != null)
                {
                    HttpContext.Session.SetString(
                        "ApiSessionCookie",
                        sessionCookie.Split(';')[0]);
                }
            }

            return user.Role == "Organiser"
                ? RedirectToAction("Dashboard", "Organiser")
                : RedirectToAction("Index", "Participant");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            string name, string surname, string email, string phoneNumber,string password)
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.PostAsJsonAsync(
                "api/Auth/register",
                new
                {
                    FirstName = name,
                    LastName = surname,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Password = password,
                    Role = "Participant"
                });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Registration failed. Please check your details.";
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}