using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.Interfaces;

public interface IConsultaService
{
    Task<Result<Consulta>> AgendarConsulta(CreateConsultaDTO dto);
    Task<Result> CancelarConsulta(Guid id, MotivosCancelamentoConsulta motivo);
    Task<Result<IEnumerable<ConsultaViewDTO>>> GetConsultasByPacienteId(int pacienteId);
    Task<Result<IEnumerable<ConsultaViewDTO>>> GetConsultasByMedicoId (int medicoId);
    Task<Result<IEnumerable<ConsultaViewDTO>>> GetConsultasByDateRange(DateTime startDate, DateTime endDate);
    
    Task<Result<IEnumerable<ConsultaViewDTO>>> GetMedicoConsultasByDateRange(int idMedico, DateTime startDate, DateTime endDate);
    
}