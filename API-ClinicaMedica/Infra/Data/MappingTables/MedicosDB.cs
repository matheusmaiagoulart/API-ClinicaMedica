using API_ClinicaMedica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_ClinicaMedica.Infra.Data.MappingTables;

public class MedicosDB : IEntityTypeConfiguration<Medico>
{
    public void Configure(EntityTypeBuilder<Medico> e)
    {
       
        {
            e.HasKey(p => p.IdMedico);
            e.HasOne(p => p.Usuario)
                .WithOne()
                .HasForeignKey<Medico>(p => p.IdMedico)
                .HasConstraintName("FK_Medico_Usuario");
            
            e.Property(p => p.Ativo).IsRequired();
            e.Property(u => u.Especialidade).HasColumnName("Especialidade").IsRequired();
            e.Property(i => i.Crm).HasColumnName("Crm").IsRequired().HasMaxLength(15);
            
        };
    }
}