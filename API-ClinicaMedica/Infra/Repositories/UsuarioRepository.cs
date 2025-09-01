using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Infra.Data.DbContext;
using API_ClinicaMedica.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_ClinicaMedica.Infra.Repositories;
public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
   
    private readonly AppDbContext _context;
    public UsuarioRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    

    public async Task<Usuario> GetUserById(int id)
    {
        var usuario = await _context.Usuarios
            .Where(u => u.IdUsuario == id)
            .FirstOrDefaultAsync();
        return usuario;
    }

    public async Task<IEnumerable<Usuario>> GetAllUsers()
    {
        var allUsers = await _context.Usuarios
            .AsNoTracking()
            .ToListAsync();
        return allUsers;
    }

    public async Task<bool> isEmailAvailable(string email)
    {
        return !await _context.Usuarios
            .AnyAsync(u => u.Email == email);
        
        
    }
    public async Task<bool> isTelefoneAvailable(string telefone)
    {
        return  !await _context.Usuarios
            .AnyAsync(u => u.InformacoesBasicas.Telefone == telefone);
        
    }
    
    public async Task<bool> isCpfAvailable(string cpf)
    {
        return !await _context.Usuarios
            .AnyAsync(u => u.InformacoesBasicas.Cpf == cpf);
    }
    
    public async Task<bool> existsById(int id)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.IdUsuario == id);

    }
    
}