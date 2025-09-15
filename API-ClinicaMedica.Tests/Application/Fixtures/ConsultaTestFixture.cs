using API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;
using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Domain.ValueObjects;
using API_ClinicaMedica.Infra.Interfaces;
using Bogus;
using Bogus.DataSets;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Fixtures;

public class ConsultaTestFixture : IDisposable
{
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Faker Faker { get; }

    // Datas fixas para testes
    public DateTime SextaFeira => new DateTime(2025, 9, 5); // Sexta-feira
    public DateTime Sabado => new DateTime(2025, 9, 6); // Sábado
    public DateTime Domingo => new DateTime(2025, 9, 7); // Domingo
    public DateTime Segunda => new DateTime(2025, 9, 8); // Segunda-feira
    public DateTime agora = DateTime.Now;

    public ConsultaTestFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        Faker = new Faker();
    }

    public HorarioFuncionamentoClinicaValidator CreateHorarioFuncionamentoClinicaValidator()
    {
        return new HorarioFuncionamentoClinicaValidator();
    }
    
    public MedicoValidoValidator CreateMedicoValidoValidator()
    {
        return new MedicoValidoValidator(MockUnitOfWork.Object);
    }
    
    public UsuarioValidoValidator CreateUsuarioValidoValidator()
    {
        return new UsuarioValidoValidator(MockUnitOfWork.Object);
    }

    public DataValidator CreateDataValidator()
    {
        return new DataValidator();
    }
    public Paciente PacienteValid(CreateConsultaDTO consultaDTO)
    {
        return new Paciente(consultaDTO.IdPaciente, false, new List<MedicamentoControlado>());
    }

    public Medico MedicoValid(CreateConsultaDTO consultaDTO, bool ativo)
    {
        return new Medico(consultaDTO.IdMedico, consultaDTO.Especialidade,
            Faker.Random.String2(5, 10), ativo, Estados.SP);
    }
    
    public ValidarHoraConsulta CreateValidarHoraConsulta()
    {
        return new ValidarHoraConsulta();
    }

    public MedicoDisponibilidadeValidator CreateMedicoDisponibilidadeValidator()
    {
        return new MedicoDisponibilidadeValidator(MockUnitOfWork.Object);
    }
    

    // Método ÚNICO e flexível - só passa o que precisa, resto é gerado automaticamente
    public CreateConsultaDTO CreateConsultaDTO(
        int? idPaciente = null,
        int? idMedico = null,
        Especialidades? especialidade = null,
        DateTime? dataHoraConsulta = null)
    {
        return new CreateConsultaDTO()
        {
            IdPaciente = idPaciente ?? Faker.Random.Int(1, 100),
            IdMedico = idMedico ?? Faker.Random.Int(1, 100),
            Especialidade = especialidade ?? Faker.PickRandom<Especialidades>(),
            DataHoraConsulta = dataHoraConsulta ?? Faker.Date.Future()
        };
    }

    public void Dispose()
    {
        // Cleanup se necessário
    }
}