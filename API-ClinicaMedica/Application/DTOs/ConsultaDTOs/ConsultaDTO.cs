using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.DTOs.ConsultaDTOs;

public class ConsultaDTO
{
    public Guid IdConsulta { get; set; }
    
    public int IdPaciente { get; set; }
    public Paciente Paciente { get;  set; }
    
    public int IdMedico { get; set; }
    public Medico Medico { get;  set; }
    public Especialidades Especialidade { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Ativo { get; set; }
    public MotivosCancelamentoConsulta? MotivoCancelamento { get; set; }
}