
using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Application.Results;
using API_ClinicaMedica.Domain.Entities;

namespace API_ClinicaMedica.Application.Services.PacienteService.Interfaces;

public interface IPacienteService
{
    Task<Result<Paciente>> CreatePaciente(CreatePacienteDTO dto);
    Task<Result<PacienteDTO>> GetPacienteById(int id);
    Task<Result<IEnumerable<PacienteDTO>>> GetAllPacientes();
    
    Task<Result> SoftDeletePaciente(int id);
    Task<Result<PacienteDTO>> UpdatePaciente(int id, UpdatePacienteDTO dto);
}