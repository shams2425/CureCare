using PatientService.BusinessLogicLayer.DTOs;

namespace PatientService.BusinessLogicLayer.ServicesContract;

public interface IPatientService
{
    Task<PatientResponse> CreatePatientAsync(PatientRequest request);
    Task<IEnumerable<PatientResponse>> GetAllPatientAsync();
    Task<PatientResponse> GetPatientByIdAsync(Guid id);
    Task<PatientResponse> UpdatePatientAsync(Guid id, UpdatePatientRequest update);
    Task<bool> DeletePatientAsync( Guid id);
}
