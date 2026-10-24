using Microsoft.AspNetCore.Mvc;
using PatientService.BusinessLogicLayer.DTOs;
using PatientService.BusinessLogicLayer.ServicesContract;

namespace PatientService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientController(IPatientService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromBody]PatientRequest request)
    {
        var patient = await _service.CreatePatientAsync(request);
        return Ok(patient);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPatient()
    {
        var patient = await _service.GetAllPatientAsync();
        return Ok(patient);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(Guid id)
    {
      var patient = await _service.GetPatientByIdAsync(id);
        if (patient == null)
        {
            return NotFound("patient not found");
        }
        return Ok(patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(Guid id , UpdatePatientRequest update)
    {
        var patient = await _service.UpdatePatientAsync(id, update);
        if (patient == null)
        {
            return NotFound("patient not found");
        }
        return Ok(patient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        var isDeleted = await _service.DeletePatientAsync(id);

        if (!isDeleted)
        {
            return NotFound("Patient not found");
        }

        return Ok("Patient deleted successfully");
    }
}
