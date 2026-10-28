
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using CureCare.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CureCare.Web.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AccountController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    // GET: User Login page
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: User Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Login(LoginViewModel model)
    {
        return LoginByRoleAsync(model, "User", nameof(Login));
    }

    // GET: Admin Login page
    [HttpGet]
    public IActionResult AdminLogin()
    {
        return View();
    }

    // POST: Admin Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> AdminLogin(LoginViewModel model)
    {
        return LoginByRoleAsync(
            model,
            "Admin",
            nameof(AdminLogin));
    }

    // Shared login logic with role verification
    private async Task<IActionResult> LoginByRoleAsync(
        LoginViewModel model,
        string expectedRole,
        string viewName)
    {
        if (!ModelState.IsValid)
            return View(viewName, model);

        try
        {
            var client = _httpClientFactory.CreateClient("AuthApi");

            var response = await client.PostAsJsonAsync(
                "api/Auth/Login",
                new
                {
                    model.Email,
                    model.Password
                });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    await GetErrorMessage(response));

                return View(viewName, model);
            }

            var loginResponse =
                await response.Content.ReadFromJsonAsync<LoginResponseModel>();

            if (loginResponse == null ||
                loginResponse.Id <= 0 ||
                string.IsNullOrWhiteSpace(loginResponse.Email) ||
                string.IsNullOrWhiteSpace(loginResponse.Role))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Login response is incomplete.");

                return View(viewName, model);
            }

            // Do not allow User accounts to enter Admin Login,
            // or Admin accounts to enter User Login.
            if (!string.Equals(
                    loginResponse.Role,
                    expectedRole,
                    StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    string.Empty,
                    expectedRole == "Admin"
                        ? "This account is not registered as an Admin."
                        : "Please use Admin Login for an Admin account.");

                return View(viewName, model);
            }

            var displayName = string.IsNullOrWhiteSpace(
                loginResponse.FullName)
                    ? loginResponse.Email
                    : loginResponse.FullName;

            var claims = new List<Claim>
            {
                new(
                    ClaimTypes.NameIdentifier,
                    loginResponse.Id.ToString()),

                new(
                    ClaimTypes.Name,
                    displayName),

                new(
                    ClaimTypes.Email,
                    loginResponse.Email),

                new(
                    ClaimTypes.Role,
                    loginResponse.Role)
            };

            var identity = new ClaimsIdentity(
                claims,
                "CureCareCookie");

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                "CureCareCookie",
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    AllowRefresh = true
                });

            TempData["SuccessMessage"] = "Login successful.";

            return RedirectToAction("Index", "Home");
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(
                string.Empty,
                "AuthService is unavailable. Please try again later.");

            return View(viewName, model);
        }
        catch (JsonException)
        {
            ModelState.AddModelError(
                string.Empty,
                "AuthService returned an invalid login response.");

            return View(viewName, model);
        }
    }

    // GET: User Registration page
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: Register a normal User
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var client = _httpClientFactory.CreateClient("AuthApi");

            var response = await client.PostAsJsonAsync(
                "api/Auth/Register",
                new
                {
                    model.FullName,
                    model.Email,
                    model.Password
                });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] =
                    "Registration successful. Please log in.";

                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(
                string.Empty,
                await GetErrorMessage(response));

            return View(model);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(
                string.Empty,
                "AuthService is unavailable. Please try again later.");

            return View(model);
        }
    }

    // GET: Admin Registration page
    [HttpGet]
    public IActionResult RegisterAdmin()
    {
        return View();
    }

    // POST: Register an Admin through AuthService
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAdmin(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var setupSecret = _configuration["AdminSetup:Secret"];

        if (string.IsNullOrWhiteSpace(setupSecret))
        {
            ModelState.AddModelError(
                string.Empty,
                "Admin registration is not configured.");

            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient("AuthApi");

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "api/Auth/RegisterAdmin")
            {
                Content = JsonContent.Create(new
                {
                    model.FullName,
                    model.Email,
                    model.Password
                })
            };

            request.Headers.Add(
                "X-Admin-Setup-Secret",
                setupSecret);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] =
                    "Admin registered successfully. Please log in.";

                return RedirectToAction(nameof(AdminLogin));
            }

            ModelState.AddModelError(
                string.Empty,
                await GetErrorMessage(response));

            return View(model);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(
                string.Empty,
                "AuthService is unavailable. Please try again later.");

            return View(model);
        }
    }

    // POST: Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CureCareCookie");

        TempData["SuccessMessage"] =
            "You have logged out successfully.";

        return RedirectToAction(nameof(Login));
    }

    // GET: Access Denied
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // Read error messages returned by AuthService
    private static async Task<string> GetErrorMessage(
        HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
        {
            return $"Request failed with status code {(int)response.StatusCode}.";
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            if (root.TryGetProperty("message", out var message) ||
                root.TryGetProperty("Message", out message))
            {
                return message.GetString()
                    ?? "The request could not be completed.";
            }

            if (root.TryGetProperty("title", out var title))
            {
                return title.GetString()
                    ?? "The request could not be completed.";
            }
        }
        catch (JsonException)
        {
            // Return a generic message for non-JSON responses.
        }

        return "The request could not be completed. Please check your details.";
    }
}
