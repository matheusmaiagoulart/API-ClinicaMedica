using API_ClinicaMedica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_ClinicaMedica.Infra.Data.MappingTables;

public class ConsultaDB : IEntityTypeConfiguration<Consulta>
{
    public void Configure(EntityTypeBuilder<Consulta> e)
    {
        {
            e.HasKey(c => c.IdConsulta);
            e.HasOne(c =>c.Medico)
                .WithMany()
                .HasForeignKey(p => p.IdMedico)
                .HasConstraintName("FK_Medico_Consulta")
                .OnDelete(DeleteBehavior.NoAction);
            e.HasOne(c => c.Paciente)
                .WithMany()
                .HasForeignKey(p => p.IdPaciente)
                .HasConstraintName("FK_Medico_Paciente")
                .OnDelete(DeleteBehavior.NoAction);

            e.Property(c => c.DataHoraConsulta).HasColumnName("DataConsulta").IsRequired();
            e.Property(c => c.IdMedico).HasColumnName("IdMedico").IsRequired();
            e.Property(c => c.IdPaciente).HasColumnName("IdPaciente").IsRequired();
            e.Property(c => c.Especialidade).HasColumnName("Especialidade").IsRequired();
            e.Property(c => c.DataHoraConsulta).HasColumnName("DataHoraConsulta").IsRequired();
            e.Property(c => c.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            e.Property(c => c.Ativo).HasColumnName("Ativo").IsRequired();
            e.Property(c => c.MotivoCancelamento).HasColumnName("MotivoCancelamento");
            

        }
       
    }
}