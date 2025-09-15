using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;
using API_ClinicaMedica.Application.FluentValidation.ConsultaValidations;
using API_ClinicaMedica.Application.FluentValidation.MedicoValidations;
using API_ClinicaMedica.Application.FluentValidation.PacienteValidations;
using API_ClinicaMedica.Application.FluentValidation.UsuarioValidations;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Domain.ValueObjects;
using Bogus;
using Bogus.Extensions.Brazil;

namespace API_ClinicaMedica.Tests.Application.Fixtures;

public class FluentValidationTestFixture : IDisposable
{
    public Faker Faker { get; }

    public FluentValidationTestFixture()
    {
        Faker = new Faker();
    }

    // Validators
    public UsuarioDTOValidator CreateUsuarioDTOValidator()
    {
        return new UsuarioDTOValidator();
    }

    public CreatePacienteDTOValidation CreatePacienteDTOValidation()
    {
        return new CreatePacienteDTOValidation();
    }

    public UpdatePacienteValidation CreateUpdatePacienteValidation()
    {
        return new UpdatePacienteValidation();
    }

    public CreateMedicoDTOValidation CreateMedicoDTOValidation()
    {
        return new CreateMedicoDTOValidation();
    }

    public CreateConsultaDTOValidations CreateConsultaDTOValidations()
    {
        return new CreateConsultaDTOValidations();
    }

    // DTOs Válidos
    public CreateUsuarioDTO CreateUsuarioDTOValid(
        string? email = null,
        string? senha = null,
        string? nome = null,
        string? cpf = null,
        string? rg = null,
        DateTime? dataNascimento = null,
        string? telefone = null,
        string? logradouro = null,
        string? numero = null,
        string? bairro = null,
        string? cidade = null,
        Estados? estado = null,
        string? cep = null)
    {
        return new CreateUsuarioDTO
        {
            Email = email ?? Faker.Internet.Email(),
            Senha = senha ?? Faker.Internet.Password(6),
            InformacoesBasicas = new InformacoesBasicasDTO()
            {
                Nome = nome ?? Faker.Person.FullName,
                Cpf = cpf ?? Faker.Person.Cpf().Replace(".", "").Replace("-", ""),
                Rg = rg ?? Faker.Random.String2(9, "0123456789"),
                DataNascimento = dataNascimento ?? Faker.Date.Past(50, DateTime.Now.AddYears(-18)),
                Telefone = telefone ?? Faker.Phone.PhoneNumber("###########").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "")
            },
            Endereco = new EnderecoDTO()
            {
                Logradouro = logradouro ?? Faker.Address.StreetName(),
                Numero = numero ?? Faker.Random.Int(1, 9999).ToString(),
                Bairro = bairro ?? Faker.Address.City(),
                Cidade = cidade ?? Faker.Address.City(),
                Estado = estado ?? Faker.PickRandom<Estados>(),
                Cep = cep ?? Faker.Random.String2(8, "0123456789")
            }
        };
    }

    // Método especial para criar DTOs com valores null para testes
    public CreateUsuarioDTO CreateUsuarioDTOWithNullValues(
        string? email = "valid@email.com",
        string? senha = "123456",
        string? nome = "Nome Válido",
        string? cpf = "12345678901",
        string? rg = "123456789",
        DateTime? dataNascimento = null,
        string? telefone = "11999999999",
        string? logradouro = "Rua Válida",
        string? numero = "123",
        string? bairro = "Bairro Válido",
        string? cidade = "Cidade Válida",
        Estados? estado = Estados.SP,
        string? cep = "12345678")
    {
        var dataDefault = dataNascimento ?? DateTime.Now.AddYears(-20);
        
        return new CreateUsuarioDTO
        {
            Email = email,
            Senha = senha,
            InformacoesBasicas = new InformacoesBasicasDTO()
            {
                Nome = nome,
                Cpf = cpf,
                Rg = rg,
                DataNascimento = dataDefault,
                Telefone = telefone
            },
            Endereco = new EnderecoDTO()
            {
                Logradouro = logradouro,
                Numero = numero,
                Bairro = bairro,
                Cidade = cidade,
                Estado = estado ?? Estados.SP,
                Cep = cep
            }
        };
    }

    public CreatePacienteDTO CreatePacienteDTOValid(
        int? idUsuario = null,
        bool? pcd = null,
        List<MedicamentoControlado>? medicamentosControlados = null)
    {
        return new CreatePacienteDTO
        {
            IdUsuario = idUsuario ?? Faker.Random.Int(1, 1000),
            Pcd = pcd ?? Faker.Random.Bool(),
            MedicamentosControlados = medicamentosControlados ?? CreateMedicamentosControladosValid()
        };
    }

    // Método especial para criar DTOs com valores null para testes
    public CreatePacienteDTO CreatePacienteDTOWithNullValues(
        int? idUsuario = 1,
        bool? pcd = true,
        List<MedicamentoControlado>? medicamentosControlados = null)
    {
        return new CreatePacienteDTO
        {
            IdUsuario = idUsuario ?? 1,
            Pcd = pcd ?? true,
            MedicamentosControlados = medicamentosControlados ?? new List<MedicamentoControlado>()
        };
    }

    public UpdatePacienteDTO CreateUpdatePacienteDTOValid(
        int? idPaciente = null,
        bool? pcd = null,
        bool? ativo = null,
        List<MedicamentoControlado>? medicamentosControlados = null)
    {
        return new UpdatePacienteDTO
        {
            IdPaciente = idPaciente ?? Faker.Random.Int(1, 1000),
            Pcd = pcd ?? Faker.Random.Bool(),
            Ativo = ativo ?? true,
            MedicamentosControlados = medicamentosControlados ?? CreateMedicamentosControladosValid()
        };
    }

    // Método especial para criar DTOs com valores null para testes
    public UpdatePacienteDTO CreateUpdatePacienteDTOWithNullValues(
        int? idPaciente = 1,
        bool? pcd = true,
        bool? ativo = true,
        List<MedicamentoControlado>? medicamentosControlados = null)
    {
        return new UpdatePacienteDTO
        {
            IdPaciente = idPaciente ?? 1,
            Pcd = pcd ?? true,
            Ativo = ativo ?? true,
            MedicamentosControlados = medicamentosControlados ?? new List<MedicamentoControlado>()
        };
    }

    public CreateMedicoDTO CreateMedicoDTOValid(
        int? idUsuario = null,
        string? especialidade = null,
        string? crmNumber = null,
        string? ufCrm = null,
        bool? ativo = null)
    {
        return new CreateMedicoDTO
        {
            IdUsuario = idUsuario ?? Faker.Random.Int(1, 1000),
            Especialidade = especialidade ?? Faker.PickRandom<Especialidades>().ToString(),
            CrmNumber = crmNumber ?? Faker.Random.String2(6, "0123456789"),
            UfCrm = ufCrm ?? Faker.PickRandom<Estados>().ToString(),
            Ativo = ativo ?? true
        };
    }

    // Método especial para criar DTOs com valores null para testes
    public CreateMedicoDTO CreateMedicoDTOWithNullValues(
        int? idUsuario = 1,
        string? especialidade = null,
        string? crmNumber = "123456",
        string? ufCrm = null,
        bool? ativo = true)
    {
        return new CreateMedicoDTO
        {
            IdUsuario = idUsuario ?? 1,
            Especialidade = especialidade ?? Especialidades.CARDIOLOGIA.ToString(),
            CrmNumber = crmNumber,
            UfCrm = ufCrm,
            Ativo = ativo ?? true
        };
    }

    public CreateConsultaDTO CreateConsultaDTOValid(
        int? idPaciente = null,
        int? idMedico = null,
        Especialidades? especialidade = null,
        DateTime? dataHoraConsulta = null)
    {
        return new CreateConsultaDTO
        {
            IdPaciente = idPaciente ?? Faker.Random.Int(1, 1000),
            IdMedico = idMedico ?? Faker.Random.Int(1, 1000),
            Especialidade = especialidade ?? Faker.PickRandom<Especialidades>(),
            DataHoraConsulta = dataHoraConsulta ?? Faker.Date.Future()
        };
    }

    // Método especial para criar DTOs com valores null para testes
    public CreateConsultaDTO CreateConsultaDTOWithNullValues(
        int? idPaciente = 1,
        int? idMedico = 1,
        Especialidades? especialidade = null,
        DateTime? dataHoraConsulta = null)
    {
        return new CreateConsultaDTO
        {
            IdPaciente = idPaciente ?? 1,
            IdMedico = idMedico ?? 1,
            Especialidade = especialidade ?? Especialidades.CARDIOLOGIA,
            DataHoraConsulta = dataHoraConsulta ?? Faker.Date.Future()
        };
    }

    // Helpers
    private List<MedicamentoControlado> CreateMedicamentosControladosValid()
    {
        return new List<MedicamentoControlado>
        {
            new MedicamentoControlado(
                Faker.Random.String2(1, 40),
                Faker.Random.String2(1, 40),
                Faker.Random.String2(1, 40),
                Faker.Random.String2(1, 150)
            )
        };
    }

    public void Dispose()
    {
        // Cleanup se necessário
    }
}
