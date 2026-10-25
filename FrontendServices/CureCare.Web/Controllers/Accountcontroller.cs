
using System.Net.Http.Json;
using System.Text.Json;
using CureCare.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CureCare.Web.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    [HttpGet]
    public IActionResult Login()
    {
        return View("~/Views/Account/Login.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

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

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Login successful.";
                return RedirectToAction("Index", "Home");
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

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

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

    private static async Task<string> GetErrorMessage(
        HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
            return $"Request failed with status code {(int)response.StatusCode}.";

        try
        {
            using var document = JsonDocument.Parse(content);

            if (document.RootElement.TryGetProperty("message", out var message))
                return message.GetString() ?? "The request could not be completed.";

            if (document.RootElement.TryGetProperty("Message", out message))
                return message.GetString() ?? "The request could not be completed.";

            if (document.RootElement.TryGetProperty("title", out var title))
                return title.GetString() ?? "The request could not be completed.";
        }
        catch (JsonException)
        {
            // Use a generic message if the response is not JSON.
        }

        return "The request could not be completed. Please check your details.";
    }
}
