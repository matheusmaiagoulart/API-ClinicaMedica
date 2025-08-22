using API_ClinicaMedica.Data;
using API_ClinicaMedica.Repositories.Implementations.UsuarioRepository;

namespace API_ClinicaMedica.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    
    private readonly AppDbContext _context;
    public IUsuarioRepository Usuarios { get; private set; }
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Usuarios = new UsuarioRepository(_context);
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