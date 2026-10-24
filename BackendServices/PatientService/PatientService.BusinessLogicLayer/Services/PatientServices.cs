using AutoMapper;
using PatientService.BusinessLogicLayer.DTOs;
using PatientService.BusinessLogicLayer.ServicesContract;
using PatientService.DataAccessLayer.Entities;
using PatientService.DataAccessLayer.RepositoriesContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PatientService.BusinessLogicLayer.Services;

public class PatientServices : IPatientService
{
    private readonly IPatientRepository _repo;
    private readonly IMapper _mapper;

    public PatientServices(IPatientRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PatientResponse> CreatePatientAsync(PatientRequest request)
    {
        var patient = _mapper.Map<Patient>(request);
        patient.Id = Guid.NewGuid();
        patient.CreatedAt = DateTime.Now;
        

       await _repo.AddAsync(patient);
       return _mapper.Map<PatientResponse>(patient);
    }

    public async Task<bool> DeletePatientAsync(Guid id)
    {
      var patient =await _repo.GetByIdAsync(id);
        if (patient == null)
        {
            return false;
        }
        await _repo.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<PatientResponse>> GetAllPatientAsync()
    {
      var patient = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<PatientResponse>>(patient);
    }

    public async Task<PatientResponse> GetPatientByIdAsync(Guid id)
    {
      var patient = await _repo.GetByIdAsync(id);
        if(patient == null)
        {
            return null;
        }
        return _mapper.Map<PatientResponse>(patient);
    }

    public async Task<PatientResponse> UpdatePatientAsync(Guid id, UpdatePatientRequest update)
    {
     var patient = await _repo.GetByIdAsync(id);
        if (patient == null)
        {
            return null;
        }
       _mapper.Map(update, patient);

        patient.UpdateAt =DateTime.Now;
        await _repo.UpdateAsync(patient);
        return _mapper.Map<PatientResponse>(patient);
    }
}
