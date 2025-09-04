using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.DTOs.ConsultaDTOs;

public class CreateConsultaDTO
{
    
    public int IdPaciente { get; set; }
    public int IdMedico { get; set; }
    public Especialidades Especialidade { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    
}