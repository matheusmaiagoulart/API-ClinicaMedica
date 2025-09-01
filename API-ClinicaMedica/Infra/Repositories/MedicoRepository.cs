using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Infra.Data.DbContext;
using API_ClinicaMedica.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_ClinicaMedica.Infra.Repositories;

public class MedicoRepository : Repository<Medico>, IMedicoRepository
{
    private readonly AppDbContext _context;
    public MedicoRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Medico?> GetMedicoById(int id)
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .Where(m => m.IdMedico == id)
            .FirstOrDefaultAsync();
    }

    public async  Task<IEnumerable<Medico?>> GetAllActiveMedicos()
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .Where(m => m.Ativo == true)
            .ToListAsync();
    }

    public async Task<IEnumerable<Medico?>> GetMedicosByEspecialidadeAndActiveTrue(Especialidades especialidade)
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .Where(m => m.Ativo == true && m.Especialidade == especialidade)
            .ToListAsync();
    }

    public async Task<IEnumerable<Medico?>> GetAllMedicos()
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Medico?>> GetMedicosByEspecialidade(Especialidades especialidade)
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .Where( m => m.Especialidade == especialidade)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> existsMedicoById(int id)
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .AnyAsync(m => m.IdMedico == id);
    }

    public async Task<Medico?> GetMedicoByCRM(string crmNumber)
    {
        return await _context.Medicos
            .Include(m => m.Usuario)
            .Where(m => m.CrmNumber == crmNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CRMExists(string crmNumber)
    {
        return await _context.Medicos
                .Where(m => m.CrmNumber == crmNumber)
                .AnyAsync();
    }
}