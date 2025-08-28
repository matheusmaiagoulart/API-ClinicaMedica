using API_ClinicaMedica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_ClinicaMedica.Infra.Data.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
    }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Medico> Medicos { get; set; }

    




}