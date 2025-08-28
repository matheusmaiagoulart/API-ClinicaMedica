using API_ClinicaMedica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_ClinicaMedica.Infra.Data.MappingTables;

public class PacientesDB : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> e)
    {
       
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
        }
    }
}
