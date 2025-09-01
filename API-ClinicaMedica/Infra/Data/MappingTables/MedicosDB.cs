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
            
            e.Property(m => m.Ativo).IsRequired();
            e.Property(m => m.Especialidade).HasColumnName("Especialidade").IsRequired();
            e.Property(m => m.UfCrm).HasColumnName("UfCrm").IsRequired();
            e.Property(m => m.CrmNumber).HasColumnName("Crm").IsRequired().HasMaxLength(15);
            
        };
    }
}