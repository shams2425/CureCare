using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using CureCare.Web.Models;

namespace CureCare.Web.Controllers;

public class PatientsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PatientsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Saare patients ki list load karta hai.
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("PatientApi");

        try
        {
            var patients = await client.GetFromJsonAsync<
                List<PatientResponseViewModel>>("api/Patient");

            return View(patients ?? new List<PatientResponseViewModel>());
        }
        catch (HttpRequestException)
        {
            return View("ApiError");
        }
        catch (System.Text.Json.JsonException)
        {
            return View("ApiError");
        }
    }

    // Naya patient register karne ka form kholta hai.
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreatePatientViewModel());
    }

    // Registration form ka data API ko bhejta hai.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePatientViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("PatientApi");

        try
        {
            var response = await client.PostAsJsonAsync(
                "api/Patient",
                model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Patient registration failed. Please try again.");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(
                string.Empty,
                "Could not connect to the PatientService API.");

            return View(model);
        }
    }

    // Selected patient ki details dikhata hai.
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var client = _httpClientFactory.CreateClient("PatientApi");

        try
        {
            var patient = await client.GetFromJsonAsync<
                PatientResponseViewModel>($"api/Patient/{id}");

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }
        catch (HttpRequestException)
        {
            return View("ApiError");
        }
        catch (System.Text.Json.JsonException)
        {
            return View("ApiError");
        }
    }

    // Existing patient ka data edit form mein load karta hai.
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var client = _httpClientFactory.CreateClient("PatientApi");

        try
        {
            var patient = await client.GetFromJsonAsync<
                PatientResponseViewModel>($"api/Patient/{id}");

            if (patient == null)
            {
                return NotFound();
            }

            var model = new UpdatePatientViewModel
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                BloodGroup = patient.BloodGroup,
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address
            };

            return View(model);
        }
        catch (HttpRequestException)
        {
            return View("ApiError");
        }
        catch (System.Text.Json.JsonException)
        {
            return View("ApiError");
        }
    }

    // Edited patient data API ko bhejta hai.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        UpdatePatientViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("PatientApi");

        try
        {
            var response = await client.PutAsJsonAsync(
                $"api/Patient/{id}",
                model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Patient update failed. Please try again.");

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(
                string.Empty,
                "Could not connect to the PatientService API.");

            return View(model);
        }
    }

    // Delete confirmation page ke liye patient load karta hai.
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var client = _httpClientFactory.CreateClient("PatientApi");

        try
        {
            var patient = await client.GetFromJsonAsync<
                PatientResponseViewModel>($"api/Patient/{id}");

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }
        catch (HttpRequestException)
        {
            return View("ApiError");
        }
        catch (System.Text.Json.JsonException)
        {
            return View("ApiError");
        }
    }

    // Confirmation ke baad patient delete karta hai.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var client = _httpClientFactory.CreateClient("PatientApi");

        try
        {
            var response = await client.DeleteAsync($"api/Patient/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("ApiError");
            }

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            return View("ApiError");
        }
    }
}