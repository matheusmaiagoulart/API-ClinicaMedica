using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Implementation;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Domain.ValueObjects;
using API_ClinicaMedica.Infra.Interfaces;
using Bogus;
using Bogus.Extensions.Brazil;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Fixtures;

public class ValidacaoInformacoesBasicasFixture : IDisposable
{

    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Faker Faker { get; }

    public ValidacaoInformacoesBasicasFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        Faker = new Faker();
    }

    public ValidacaoCpf CreateValidacaoCpf()
    {
        return new ValidacaoCpf(MockUnitOfWork.Object);
    }

    public ValidacaoEmail CreateValidacaoEmail()
    {
        return new ValidacaoEmail(MockUnitOfWork.Object);
    }

    public ValidacaoTelefone CreateValidacaoTelefone()
    {
        return new ValidacaoTelefone(MockUnitOfWork.Object);
    }

public UniqueFieldsValidationDTO CreateUniqueFieldsValidationDTO(int? idUsuario = null, string? telefone = null)
    {
        return new UniqueFieldsValidationDTO
        {
            IdUsuario = idUsuario ?? Faker.Random.Int(1, 100),
            Email = Faker.Internet.Email(),
            InformacoesBasicas = new()
            {
                Cpf = Faker.Person.Cpf(),
                Telefone = telefone ?? Faker.Phone.PhoneNumber("###########")
            }
        };
    }
    
    public Usuario CreateUsuario(int? idUsuario = null, string? telefone = null)
    {
        return new Usuario
        (
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            new InformacoesBasicas( // ✅ Instanciar classe correta
                Faker.Person.FullName,
                telefone ?? Faker.Phone.PhoneNumber("###########"),
                Faker.Person.DateOfBirth,
                Faker.Person.Cpf(),
                Faker.Random.String(1, 9)
            ),
            new Endereco
            (
                Faker.Address.StreetName(),
                Faker.Random.Int(1, 1000).ToString(),
                Faker.Address.SecondaryAddress(),
                Faker.Address.StreetSuffix(),
                Faker.Address.City(),
                Faker.PickRandom<Estados>(),
                Faker.Address.ZipCode("########")
            )
        );
    }
    public void Dispose()
    {
        // TODO release managed resources here
    }
}