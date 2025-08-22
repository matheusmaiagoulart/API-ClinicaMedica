namespace API_ClinicaMedica.Repositories.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    Task<bool> CommitAsync();
    Task DisposeAsync();

}