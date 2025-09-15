using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Implementation;
using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;
using API_ClinicaMedica.Application.Services;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Domain.ValueObjects;
using API_ClinicaMedica.Infra.Interfaces;
using AutoMapper;
using Bogus;
using Bogus.Extensions.Brazil;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Fixtures;

public class UsuarioServiceFixture : IDisposable
{
    public Mock<IUnitOfWork> MockUnitOfWork { get; set; }
    public Mock<IMapper> MockMapper { get; set; }

    public Faker Faker { get; }
    public Mock<IValidacaoInformacoesBasicas> MockValidacaoCpf { get; set; }
    public Mock<IValidacaoInformacoesBasicas> MockValidacaoEmail { get; set; }
    public Mock<IValidacaoInformacoesBasicas> MockValidacaoTelefone { get; set; }

    private List<IValidacaoInformacoesBasicas> _listaValidacoes;

    public UsuarioServiceFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        MockMapper = new Mock<IMapper>();
        Faker = new Faker();

        MockValidacaoCpf = new Mock<IValidacaoInformacoesBasicas>();
        MockValidacaoEmail = new Mock<IValidacaoInformacoesBasicas>();
        MockValidacaoTelefone = new Mock<IValidacaoInformacoesBasicas>();

        _listaValidacoes = new List<IValidacaoInformacoesBasicas>
        {
            MockValidacaoCpf.Object,
            MockValidacaoEmail.Object,
            MockValidacaoTelefone.Object
        };
    }

    public UsuarioService CreateUsuarioService()
    {
        return new UsuarioService(
            MockUnitOfWork.Object,
            MockMapper.Object,
            _listaValidacoes);
    }

    public CreateUsuarioDTO CreateUsuarioDTOValid(string? telefone = null)
    {
        return new CreateUsuarioDTO
        {
            Email = Faker.Internet.Email(),
            Senha = Faker.Internet.Password(),
            InformacoesBasicas = new InformacoesBasicasDTO
            {
                Nome = Faker.Person.FullName,
                Telefone = telefone ?? Faker.Phone.PhoneNumber("###########"),
                DataNascimento = Faker.Person.DateOfBirth,
                Cpf = Faker.Person.Cpf(),
                Rg = Faker.Random.String(1, 9)
            },
            Endereco = new EnderecoDTO
            {
                Logradouro = Faker.Address.StreetName(),
                Numero = Faker.Random.Int(1, 1000).ToString(),
                Complemento = Faker.Address.SecondaryAddress(),
                Bairro = Faker.Address.StreetSuffix(),
                Cidade = Faker.Address.City(),
                Estado = Faker.PickRandom<Estados>(),
                Cep = Faker.Address.ZipCode("########")
            }
        };
    }

    public Usuario CreateUsuarioValid(string? telefone = null, string? senha = null)
    {
        return new Faker<Usuario>().CustomInstantiator(f => new Usuario(
            f.Internet.Email(),
            senha ?? f.Internet.Password(),
            new InformacoesBasicas(
                f.Person.FullName,
                telefone ?? f.Phone.PhoneNumber("###########"),
                f.Person.DateOfBirth,
                f.Person.Cpf(),
                f.Random.String(1, 9)
            ),
            new Endereco(
                f.Address.StreetName(),
                f.Random.Int(1, 1000).ToString(),
                f.Address.SecondaryAddress(),
                f.Address.StreetSuffix(),
                f.Address.City(),
                f.PickRandom<Estados>(),
                f.Address.ZipCode("########")
            )
        )).RuleFor(u => u.IdUsuario, 1).Generate();
    }
    
    public UpdateUsuarioDTO CreateUpdateUsuarioDTOValid(int? idUsuario = null, string? telefone = null)
    {
        return new UpdateUsuarioDTO
        {
            IdUsuario = idUsuario ?? 1,
            Email = Faker.Internet.Email(),
            Senha = Faker.Internet.Password(),
            InformacoesBasicas = new InformacoesBasicasUpdateDTO
            {
                Nome = Faker.Person.FullName,
                Telefone = telefone ?? Faker.Phone.PhoneNumber("###########"),
                DataNascimento = Faker.Person.DateOfBirth,
                Cpf = Faker.Person.Cpf(),
                Rg = Faker.Random.String(1, 9)
            },
            Endereco = new EnderecoUpdateDTO
            {
                Logradouro = Faker.Address.StreetName(),
                Numero = Faker.Random.Int(1, 1000).ToString(),
                Complemento = Faker.Address.SecondaryAddress(),
                Bairro = Faker.Address.StreetSuffix(),
                Cidade = Faker.Address.City(),
                Estado = Faker.PickRandom<Estados>(),
                Cep = Faker.Address.ZipCode("########")
            }
        };
    }
    
    public UsuarioDTO CreateUsuarioDTOValid(Usuario? dto)
    {
        return new UsuarioDTO
        {
            IdUsuario = dto.IdUsuario,
            Email = dto.Email,
            Senha = dto.Senha,
            InformacoesBasicas = new InformacoesBasicas
            (
                dto.InformacoesBasicas.Nome,
                dto.InformacoesBasicas.Telefone,
                dto.InformacoesBasicas.DataNascimento,
                dto.InformacoesBasicas.Cpf,
                 dto.InformacoesBasicas.Rg 
            ),
            Endereco = new Endereco
            (
                 dto.Endereco.Logradouro,
                 dto.Endereco.Numero,
                dto.Endereco.Complemento,
                 dto.Endereco.Bairro,
                    dto.Endereco.Cidade,
                    dto.Endereco.Estado,
                    dto.Endereco.Cep
            )
        };
    }

    public IEnumerable<Usuario> listaUsuarios()
    {
        var lista = new List<Usuario>();
        for (int i = 0; i < 5; i++)
        {
            lista.Add(CreateUsuarioValid());
        }
        
        return lista;
    }


public UniqueFieldsValidationDTO CreateUniqueFieldsValidationDTO(int? idUsuario = null, string? email = null, string? telefone = null)
    {
        return new UniqueFieldsValidationDTO
        {
            IdUsuario = idUsuario ??  1,
            Email = email ?? Faker.Internet.Email(),
            InformacoesBasicas = new InformacoesBasicasFieldValidationDTO
            {
            Cpf = Faker.Person.Cpf(),
            Telefone = telefone ?? Faker.Phone.PhoneNumber("###########")
        }
            };
    }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }
}