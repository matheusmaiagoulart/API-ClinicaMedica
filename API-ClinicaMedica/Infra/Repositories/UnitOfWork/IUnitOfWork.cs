using API_ClinicaMedica.Infra.Repositories.Interfaces.MedicosRepository;
using API_ClinicaMedica.Infra.Repositories.Interfaces.PacienteRepository;
using API_ClinicaMedica.Infra.Repositories.Interfaces.UsuarioRepository;

namespace API_ClinicaMedica.Infra.Repositories.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    IPacienteRepository Pacientes { get; }
    IMedicoRepository Medicos { get; }
    Task<bool> CommitAsync();
    Task DisposeAsync();

}