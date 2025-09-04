using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Domain.Entities;

public class Consulta
{
    [Key]
    public Guid IdConsulta { get; private set; }
    
    public int IdPaciente { get; private set; }
    public Paciente Paciente { get; private set; }
    
    public int IdMedico { get; private set; }
    public Medico Medico { get; private set; }
    public Especialidades Especialidade { get; private set; }
    public DateTime DataHoraConsulta { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Ativo { get; private set; }
    public MotivosCancelamentoConsulta? MotivoCancelamento { get; private set; }
    
    
    protected Consulta() { }
    
    public Consulta(int idPaciente, int idMedico, Especialidades especialidade, DateTime dataHoraConsulta, MotivosCancelamentoConsulta? motivoCancelamento = null)
    {
        string formatoPersonalizado = "dd-MM-yyyy HH:mm";
            
        IdConsulta = Guid.NewGuid();
        IdPaciente = idPaciente;
        IdMedico = idMedico;
        Especialidade = especialidade;
        DataHoraConsulta = DateTime.Parse(dataHoraConsulta.ToString(formatoPersonalizado));
        CreatedAt = DateTime.UtcNow;
        Ativo = true;
        MotivoCancelamento = motivoCancelamento;
    }
    
    public void CancelarConsulta(MotivosCancelamentoConsulta motivo)
    {
        Ativo = false;
        MotivoCancelamento = motivo;
    }
    
    
}
