using API_ClinicaMedica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_ClinicaMedica.Infra.Data.MappingTables;

public class UsuariosDB : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> e)
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
    
    }
}