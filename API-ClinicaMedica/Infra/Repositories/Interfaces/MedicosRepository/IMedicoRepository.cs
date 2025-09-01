using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Infra.Repositories.Interfaces.MedicosRepository;

public interface IMedicoRepository : IRepository<Medico>
{
    Task<Medico?> GetMedicoById(int id);
    
    Task<IEnumerable<Medico?>> GetAllActiveMedicos();
    
    Task<IEnumerable<Medico?>> GetMedicosByEspecialidadeAndActiveTrue(Especialidades especialidade);
    
    Task<IEnumerable<Medico?>> GetAllMedicos();
    
    Task<IEnumerable<Medico?>> GetMedicosByEspecialidade(Especialidades especialidade);
    
    Task<bool> existsMedicoById(int id);
    
    Task<Medico?> GetMedicoByCRM(string crmNumber);
    
    Task<bool> CRMExists(string crmNumber);
    
}