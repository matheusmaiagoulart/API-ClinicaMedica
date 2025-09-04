namespace API_ClinicaMedica.Infra.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    IPacienteRepository Pacientes { get; }
    IMedicoRepository Medicos { get; }
    IConsultaRepository Consultas { get; }
    Task<bool> CommitAsync();
    Task DisposeAsync();

}