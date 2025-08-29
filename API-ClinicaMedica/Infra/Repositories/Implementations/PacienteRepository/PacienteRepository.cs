using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Infra.Data.DbContext;
using API_ClinicaMedica.Infra.Repositories.Interfaces.PacienteRepository;
using Microsoft.EntityFrameworkCore;

namespace API_ClinicaMedica.Infra.Repositories.Implementations.PacienteRepository;

public class PacienteRepository : Repository<Paciente>, IPacienteRepository
{
    private readonly AppDbContext _context;
    public PacienteRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Paciente?> GetPacienteById(int id)
    {
        return await _context.Pacientes
            .Include(p => p.Usuario)
            .Where(p => p.IdPaciente == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Paciente?>> GetAllPacientes()
    {
        return await _context.Pacientes
            .Include(p => p.Usuario)
            .AsNoTracking()
            .ToListAsync();
    }
    

    public async Task<bool> existsById(int id)
    {
        var exists = await _context.Pacientes.AnyAsync(p => p.IdPaciente == id);
        
        return exists;
    }
}