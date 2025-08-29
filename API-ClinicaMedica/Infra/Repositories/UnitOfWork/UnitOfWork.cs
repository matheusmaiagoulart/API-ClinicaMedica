using API_ClinicaMedica.Infra.Data.DbContext;
using API_ClinicaMedica.Infra.Repositories.Implementations.PacienteRepository;
using API_ClinicaMedica.Infra.Repositories.Implementations.UsuarioRepository;
using API_ClinicaMedica.Infra.Repositories.Interfaces.PacienteRepository;
using API_ClinicaMedica.Infra.Repositories.Interfaces.UsuarioRepository;


namespace API_ClinicaMedica.Infra.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    
    private readonly AppDbContext _context;
    public IUsuarioRepository Usuarios { get; private set; }
    public IPacienteRepository Pacientes { get; private set; }
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Usuarios = new UsuarioRepository(_context);
        Pacientes = new PacienteRepository(_context);
    }

    

    public async Task<bool> CommitAsync()
    {
        var result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            return true;
        }
            return false;
        
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    public void Dispose()
    {
        _context.Dispose();

    }
}