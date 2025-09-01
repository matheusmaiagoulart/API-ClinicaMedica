using API_ClinicaMedica.Domain.Entities;

namespace API_ClinicaMedica.Infra.Interfaces;

public interface IPacienteRepository : IRepository<Paciente>
{
    Task<Paciente?> GetPacienteById(int id);
    
    Task<IEnumerable<Paciente?>> GetAllPacientes();
    
    Task<bool> existsById(int id);
    
    
    
}