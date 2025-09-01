using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.Services.MedicoService.Interfaces;

public interface IMedicoService
{
    Task<Result<Medico>> CreateMedico(CreateMedicoDTO dto);
    Task<Result<MedicoDTO>> GetMedicoById(int id);
    Task<Result<IEnumerable<MedicoDTO>>> GetAllActiveMedicos();
    Task<Result<IEnumerable<MedicoDTO>>> GetMedicosByEspecialidadeAndActiveTrue(Especialidades especialidades);
    Task<Result<IEnumerable<MedicoDTO>>> GetAllMedicos();
    Task<Result<IEnumerable<MedicoDTO>>> GetMedicosByEspecialidade(Especialidades especialidade);
    Task<Result<MedicoDTO>> GetMedicoByCRM(string crmNumber);
    Task<Result<bool>> ExistsMedicoById(int id);
    Task<Result> SoftDeleteMedico(int id);
    //Task<Result<MedicoDTO>> UpdateMedico(int id, UpdateMedicoDTO dto);
}