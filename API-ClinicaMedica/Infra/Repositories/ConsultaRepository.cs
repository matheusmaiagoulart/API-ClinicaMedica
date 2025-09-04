using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Infra.Data.DbContext;
using API_ClinicaMedica.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_ClinicaMedica.Infra.Repositories;

public class ConsultaRepository : Repository<Consulta>, IConsultaRepository
{
    private readonly AppDbContext _context;

    public ConsultaRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> VerificarDisponibilidadeConsulta(int IdMedico, DateTime DataConsulta,
        Especialidades especialidade)
    {
        var result = await _context.Consultas
            .Include(c => c.Medico)
            .ThenInclude(m => m.Usuario)
            .Where(c => c.IdMedico == IdMedico
                        && c.DataHoraConsulta.Date == DataConsulta.Date
                        && c.DataHoraConsulta.Hour == DataConsulta.Hour
                        && c.DataHoraConsulta.Minute == DataConsulta.Minute
                        && c.Medico.Especialidade == especialidade)
            .AnyAsync();

        return !result;
    }

    public async Task<Medico?> EscolheMedicoAleatorio(DateTime DataConsulta, Especialidades especialidade)
    {
        var resultRandom = await _context.Consultas
            .Include(c => c.Medico)
            .ThenInclude(m => m.Usuario)
            .Where(c => c.DataHoraConsulta == DataConsulta
                        && c.Medico.Especialidade == especialidade)
            .FirstOrDefaultAsync();

        return resultRandom.Medico;
    }
    
    public async Task<bool> PacienteLivreNoHorario(int idPaciente, DateTime dataHoraConsulta)
    {
        var consultaExiste = await _context.Consultas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .Where(c => (c.Paciente.IdPaciente == idPaciente) 
                        && c.DataHoraConsulta.Date == dataHoraConsulta.Date
                        && c.DataHoraConsulta.Hour == dataHoraConsulta.Hour
                        && c.DataHoraConsulta.Minute == dataHoraConsulta.Minute
                        && c.Ativo)
            .AnyAsync();

        return !consultaExiste;
    }

    public async Task<Consulta?> GetConsultaById(Guid id)
    {
        return await _context.Consultas
            .Include(c => c.Paciente)
                .ThenInclude(p => p.Usuario)
                    .ThenInclude(u => u.InformacoesBasicas)
            .Include(c => c.Medico)
                .ThenInclude(m => m.Usuario)
                    .ThenInclude(u => u.InformacoesBasicas)
            .FirstOrDefaultAsync(c => c.IdConsulta == id);
    }

    public async Task<IEnumerable<Consulta>> GetConsultasByPacienteId(int pacienteId)
    {
        return await _context.Consultas
            .Include(c => c.Paciente)
                .ThenInclude(p => p.Usuario)
                    .ThenInclude(u => u.InformacoesBasicas)
            .Include(c => c.Medico)
                .ThenInclude(m => m.Usuario)
                    .ThenInclude(u => u.InformacoesBasicas)
            .Where(c => c.IdPaciente == pacienteId)
            .OrderByDescending(c => c.DataHoraConsulta)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Consulta>> GetConsultasByMedicoId(int medicoId)
    {
        return await _context.Consultas
            .Include(c => c.Paciente)
            .ThenInclude(p => p.Usuario)
            .ThenInclude(u => u.InformacoesBasicas)
            .Include(c => c.Medico)
            .ThenInclude(m => m.Usuario)
            .ThenInclude(u => u.InformacoesBasicas)
            .Where(c => c.IdMedico == medicoId)
            .OrderByDescending(c => c.DataHoraConsulta)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Consulta>> GetConsultasByDateRange(DateTime startDate, DateTime endDate)
    {
        return await _context.Consultas
            .Include(c => c.Paciente)
            .ThenInclude(p => p.Usuario)
            .ThenInclude(u => u.InformacoesBasicas)
            .Include(c => c.Medico)
            .ThenInclude(m => m.Usuario)
            .ThenInclude(u => u.InformacoesBasicas)
            .Where(c => c.DataHoraConsulta.Date >= startDate && c.DataHoraConsulta.Date <= endDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Consulta>> GetMedicoConsultasByDateRange(int idMedico, DateTime startDate, DateTime endDate)
    {
        return await _context.Consultas
            .Include(c => c.Paciente)
            .Include(c => c.Medico)
            .Where(c => c.DataHoraConsulta.Date >= startDate && c.DataHoraConsulta.Date <= endDate && c.IdMedico == idMedico)
            .AsNoTracking()
            .ToListAsync();
    }
}