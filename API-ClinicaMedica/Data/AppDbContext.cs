using API_ClinicaMedica.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace API_ClinicaMedica.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Usuario>(e =>
            {
                e.HasKey(u => u.IdUsuario);
                e.Property(u => u.Email).IsRequired().HasMaxLength(100);
                e.HasIndex(i => i.Email).IsUnique();
                e.OwnsOne(u => u.InformacoesBasicas, info =>
                {
                    info.Property(i => i.Telefone).HasColumnName("Telefone").IsRequired().IsRequired().HasMaxLength(11);
                    info.Property(i => i.DataNascimento).HasColumnName("DataNascimento").IsRequired();
                    info.Property(i => i.Nome).HasColumnName("Nome").IsRequired().HasMaxLength(100);

                    info.Property(i => i.Cpf).HasColumnName("Cpf").IsRequired().HasMaxLength(11);
                    info.HasIndex(i => i.Cpf).IsUnique();

                    info.Property(i => i.Rg).HasColumnName("Rg").IsRequired().HasMaxLength(9);
                    info.HasIndex(i => i.Rg).IsUnique();

                });
                e.OwnsOne(u => u.Endereco, endereco =>
                {
                    endereco.Property(e => e.Logradouro).HasColumnName("Logradouro").IsRequired().HasMaxLength(200);
                    endereco.Property(e => e.Numero).HasColumnName("Numero").IsRequired().HasMaxLength(10);
                    endereco.Property(e => e.Complemento).HasColumnName("Complemento").HasMaxLength(100);
                    endereco.Property(e => e.Bairro).HasColumnName("Bairro").IsRequired().HasMaxLength(100);
                    endereco.Property(e => e.Cidade).HasColumnName("Cidade").IsRequired().HasMaxLength(100);
                    endereco.Property(e => e.Estado).HasColumnName("Estado").IsRequired();
                    endereco.Property(e => e.Cep).HasColumnName("Cep").IsRequired().HasMaxLength(8);
                });
            });

        modelBuilder.Entity<Paciente>(e =>
        {
            e.HasKey(p => p.IdPaciente);
            e.HasOne(p => p.Usuario)
                .WithOne()
                .HasForeignKey<Paciente>(p => p.IdPaciente)
                .HasConstraintName("FK_Paciente_Usuario");
            
            e.Property(p => p.Ativo).IsRequired();
            e.Property(i => i.Pcd).HasColumnName("Pcd").IsRequired();
            e.OwnsMany(p => p.MedicamentosControlados, mc =>
            {
                mc.ToTable("MedicamentoControlado");
                mc.WithOwner().HasForeignKey("PacienteId"); // Chave estrangeira para Paciente
                mc.Property<int>("Id"); // Shadow property para PK
                mc.HasKey("Id");
                mc.Property(m => m.Nome).HasColumnName("Nome").IsRequired().HasMaxLength(100);
                mc.Property(m => m.Dosagem).HasColumnName("Dosagem").HasMaxLength(50);
                mc.Property(m => m.Frequencia).HasColumnName("Frequencia").HasMaxLength(50);
                mc.Property(m => m.Observacoes).HasColumnName("Observacoes").HasMaxLength(200);
            });

        });
        
        
        modelBuilder.Entity<Medico>(e =>
        {
            e.HasKey(p => p.IdMedico);
            e.HasOne(p => p.Usuario)
                .WithOne()
                .HasForeignKey<Medico>(p => p.IdMedico)
                .HasConstraintName("FK_Medico_Usuario");
            
            e.Property(p => p.Ativo).IsRequired();
            e.Property(u => u.Especialidade).HasColumnName("Especialidade").IsRequired();
            e.Property(i => i.Crm).HasColumnName("Crm").IsRequired().HasMaxLength(15);
            
        });
    }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Medico> Medicos { get; set; }

    




}