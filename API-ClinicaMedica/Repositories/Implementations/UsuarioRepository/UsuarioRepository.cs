using API_ClinicaMedica.Data;
using API_ClinicaMedica.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using API_ClinicaMedica.Domain.Entities;
namespace API_ClinicaMedica.Repositories.Implementations.UsuarioRepository;
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
        bool isValid = await _context.Usuarios
            .AnyAsync(u => u.Email == email);
        if (isValid)
        {
            return false;
        }
        return true;
    }
    public async Task<bool> isTelefoneAvailable(string telefone)
    {
        bool isValid = await _context.Usuarios
            .AnyAsync(u => u.InformacoesBasicas.Telefone == telefone);
        if (isValid)
        {
            return false;
        }
        return true;
    }
    
    public async Task<bool> isCpfAvailable(string cpf)
    {

        var isAvailable = await _context.Usuarios.AnyAsync(u => u.InformacoesBasicas.Cpf == cpf);
        if (isAvailable)
        {
            return false;
        }

        return true;
    }
    
}