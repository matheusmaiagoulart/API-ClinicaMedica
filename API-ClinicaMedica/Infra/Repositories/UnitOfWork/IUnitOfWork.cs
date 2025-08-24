using API_ClinicaMedica.Infra.Repositories.Interfaces.UsuarioRepository;

namespace API_ClinicaMedica.Infra.Repositories.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    Task<bool> CommitAsync();
    Task DisposeAsync();

}