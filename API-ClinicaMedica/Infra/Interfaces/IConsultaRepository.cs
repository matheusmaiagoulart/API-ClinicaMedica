using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Infra.Interfaces;

public interface IConsultaRepository : IRepository<Consulta>
{
    Task<bool> VerificarDisponibilidadeConsulta(int IdMedico, DateTime DataConsulta, Especialidades especialidade);
    
    Task<Medico?> EscolheMedicoAleatorio(DateTime DataConsulta, Especialidades especialidade);

    Task<bool> PacienteLivreNoHorario(int idPaciente, DateTime dataHoraConsulta);
    
    Task<Consulta?> GetConsultaById(Guid id);
    
    Task<IEnumerable<Consulta>> GetConsultasByPacienteId(int pacienteId);
    Task<IEnumerable<Consulta>> GetConsultasByMedicoId(int medicoId);

    Task<IEnumerable<Consulta>> GetConsultasByDateRange(DateTime startDate, DateTime endDate);
    
    Task<IEnumerable<Consulta>> GetMedicoConsultasByDateRange(int idMedico, DateTime startDate, DateTime endDate);
    
    
}