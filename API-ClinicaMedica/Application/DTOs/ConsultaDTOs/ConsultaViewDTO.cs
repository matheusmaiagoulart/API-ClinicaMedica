using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.DTOs.ConsultaDTOs;

public class ConsultaViewDTO
{
    
    public Guid IdConsulta { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public Especialidades Especialidade { get; set; }
    public bool Ativo { get; set; }
    public MotivosCancelamentoConsulta? MotivoCancelamento { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public PacienteConsultaViewDTO Paciente { get; set; }
    public MedicoConsultaViewDTO Medico { get; set; }
}

public class PacienteConsultaViewDTO
{
    public int IdPaciente { get; set; }
    public UsuarioConsultaViewDTO Usuario { get; set; }
    public bool Pcd { get; set; }
}

public class MedicoConsultaViewDTO
{
    public int IdMedico { get; set; }
    public UsuarioConsultaViewDTO Usuario { get; set; }
    public string CrmNumber { get; set; }
}

public class UsuarioConsultaViewDTO
{
    public string Email { get; set; }
    [JsonPropertyName("InformacoesBasicas")]
    public InformacoesBasicasConsultaViewDTO InformacoesBasicas { get; set; }
}

public class InformacoesBasicasConsultaViewDTO
{
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }

}

