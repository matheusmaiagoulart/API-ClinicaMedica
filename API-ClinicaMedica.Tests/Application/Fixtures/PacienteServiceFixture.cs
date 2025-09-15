using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
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

public class PacienteServiceFixture : IDisposable
{
    
    public Mock<IUnitOfWork> MockUnitOfWork { get; set; }
    public Mock<IMapper> MockMapper { get; set; }
    public Faker Faker { get; }

    public PacienteServiceFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        MockMapper = new Mock<IMapper>();
        Faker = new Faker();
    }
    public List<MedicamentoControlado> listaMedicamentosControlados = new List<MedicamentoControlado>
    {
        new MedicamentoControlado
        (
            "Medicamento A",
            "500mg",
            "2 vezes ao dia"
        ),
        new MedicamentoControlado
        (
            "Medicamento B",
            "250mg",
            "1 vez ao dia"
        )
    };
    
    public PacienteService CreatePacienteService()
    {
        return new PacienteService(
            MockUnitOfWork.Object,
            MockMapper.Object);
    }
    
    // Criando DTO Valido
    public CreatePacienteDTO CreatePacienteDTOValid(int? id = null)
    {
        return new CreatePacienteDTO()
        {

            IdUsuario = id ?? Faker.Random.Int(1, 1000),
            Pcd = Faker.Random.Bool(),
            MedicamentosControlados = listaMedicamentosControlados
            
        };
    }
    
    public UpdatePacienteDTO CreateUpdatePacienteDTOValid(int? id = null, bool? pcd = null, MedicamentoControlado? medicamentoControlado = null)
    {
        return new UpdatePacienteDTO()
        {

            IdPaciente = id ?? Faker.Random.Int(1, 1000),
            Pcd = pcd ?? Faker.Random.Bool(),
            Ativo = true,
            MedicamentosControlados = listaMedicamentosControlados
            
        };
    }
    
    public PacienteDTO PacienteDTOValid(int? id = null, Usuario? usuario = null, bool? pcd = null, bool? ativo = null, IReadOnlyCollection<MedicamentoControlado?> medicamentoControlado = null)
    {
        return new PacienteDTO
        {

            IdPaciente = id ?? Faker.Random.Int(1, 1000),
            Usuario = usuario ?? CreateUsuarioValid(),
            Pcd = pcd ?? Faker.Random.Bool(),
            Ativo = ativo ?? true,
            MedicamentosControlados = medicamentoControlado ?? listaMedicamentosControlados
            
        };
    }

    public IEnumerable<Paciente> GerarPacientesDTO(int quantidade) 
    {
        var listPacientes = new List<Paciente>();
        for (int i = 1; i < quantidade; i++)
        {
            listPacientes.Add(CreatePacienteValid(i, true));
        }

        return listPacientes;
    }
    
    public Paciente CreatePacienteValid(int? id = null, bool? ativo = null)
    {
        return new Paciente
        (
            id ?? Faker.Random.Int(1, 1000),
            Faker.Random.Bool(),
            listaMedicamentosControlados
            
        );
    }
    
    public Usuario CreateUsuarioValid(string? telefone = null)
    {
        return new Faker<Usuario>().CustomInstantiator(f => new Usuario(
            f.Internet.Email(),
            f.Internet.Password(),
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



    public void Dispose()
    {
        // TODO release managed resources here
    }
}