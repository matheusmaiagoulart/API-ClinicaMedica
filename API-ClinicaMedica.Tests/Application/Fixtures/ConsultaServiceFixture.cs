using API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;
using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Services;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Infra.Interfaces;
using AutoMapper;
using Bogus;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Fixtures;

public class ConsultaServiceFixture : IDisposable
{
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Mock<IMapper> MockMapper { get; set; }
    public Mock<IMarcarConsultaValidator> MockValidarHoraConsulta { get; set; }
    public Mock<IMarcarConsultaValidator> MockMarcarConsultaValidator { get; set; }
    public Faker Faker { get; }

    public ConsultaServiceFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        MockMapper = new Mock<IMapper>();
        MockValidarHoraConsulta = new Mock<IMarcarConsultaValidator>();
        MockMarcarConsultaValidator = new Mock<IMarcarConsultaValidator>();
        Faker = new Faker();
    }

    public ConsultaService CreateConsultaService()
    {
        var validators = new List<IMarcarConsultaValidator> { MockMarcarConsultaValidator.Object };
        
        // Criando uma instância real de ValidarHoraConsulta para simplificar os testes
        var validarHoraConsulta = new ValidarHoraConsulta();
        
        return new ConsultaService(MockMapper.Object, validators, MockUnitOfWork.Object, validarHoraConsulta);
    }

    public CreateConsultaDTO CreateConsultaDTOValid(int? idPaciente = null, int? idMedico = null, DateTime? dataHoraConsulta = null, Especialidades? especialidade = null)
    {
        // Criando uma data/hora válida para consulta usando um dos horários permitidos
        DateTime dataConsulta;
        if (dataHoraConsulta.HasValue)
        {
            dataConsulta = dataHoraConsulta.Value;
        }
        else
        {
            // Usando um horário válido específico: 8:00 da manhã (primeiro horário válido)
            var dataBase = DateTime.Now.AddDays(1).Date;
            dataConsulta = dataBase.Add(new TimeSpan(8, 0, 0)); // 8:00h é definitivamente válido
        }
        
        return new CreateConsultaDTO
        {
            IdPaciente = idPaciente ?? Faker.Random.Int(1, 1000),
            IdMedico = idMedico ?? Faker.Random.Int(1, 1000),
            DataHoraConsulta = dataConsulta,
            Especialidade = especialidade ?? Especialidades.CARDIOLOGIA
        };
    }

    public Consulta ConsultaValid(CreateConsultaDTO dto)
    {
        return new Consulta(
            dto.IdPaciente,
            dto.IdMedico,
            dto.Especialidade,
            dto.DataHoraConsulta
        );
    }

    public ConsultaViewDTO CreateConsultaViewDTO(Consulta consulta)
    {
        return new ConsultaViewDTO
        {
            IdConsulta = consulta.IdConsulta,
            Paciente = new PacienteConsultaViewDTO
            {
                IdPaciente = Faker.Random.Int(1, 1000),
                Pcd = false,
                Usuario = new UsuarioConsultaViewDTO
                {
                    Email = Faker.Internet.Email(),
                    InformacoesBasicas = new InformacoesBasicasConsultaViewDTO
                    {
                        Nome = Faker.Name.FullName(),
                        DataNascimento = Faker.Date.Past(50, DateTime.Now.AddYears(-18))
                    }
                }
            },
            Medico = new MedicoConsultaViewDTO
            {
                IdMedico = Faker.Random.Int(1, 1000),
                CrmNumber = Faker.Random.Replace("#####"),
                Usuario = new UsuarioConsultaViewDTO
                {
                    Email = Faker.Internet.Email(),
                    InformacoesBasicas = new InformacoesBasicasConsultaViewDTO
                    {
                        Nome = Faker.Name.FullName(),
                        DataNascimento = Faker.Date.Past(50, DateTime.Now.AddYears(-25))
                    }
                }
            },
            DataHoraConsulta = consulta.DataHoraConsulta,
            Especialidade = consulta.Especialidade,
            Ativo = consulta.Ativo,
            MotivoCancelamento = consulta.MotivoCancelamento
        };
    }

    public IEnumerable<ConsultaViewDTO> CreateListConsultasViewDTO(int count)
    {
        var consultas = new List<ConsultaViewDTO>();
        for (int i = 0; i < count; i++)
        {
            consultas.Add(new ConsultaViewDTO
            {
                IdConsulta = Guid.NewGuid(),
                DataHoraConsulta = DateTime.Now.AddDays(Faker.Random.Int(1, 30)),
                Especialidade = Especialidades.CARDIOLOGIA,
                Ativo = true,
                MotivoCancelamento = null,
                CreatedAt = DateTime.Now,
                Paciente = new PacienteConsultaViewDTO
                {
                    IdPaciente = Faker.Random.Int(1, 1000),
                    Pcd = false,
                    Usuario = new UsuarioConsultaViewDTO
                    {
                        Email = Faker.Internet.Email(),
                        InformacoesBasicas = new InformacoesBasicasConsultaViewDTO
                        {
                            Nome = Faker.Name.FullName(),
                            DataNascimento = Faker.Date.Past(50, DateTime.Now.AddYears(-18))
                        }
                    }
                },
                Medico = new MedicoConsultaViewDTO
                {
                    IdMedico = Faker.Random.Int(1, 1000),
                    CrmNumber = Faker.Random.Replace("#####"),
                    Usuario = new UsuarioConsultaViewDTO
                    {
                        Email = Faker.Internet.Email(),
                        InformacoesBasicas = new InformacoesBasicasConsultaViewDTO
                        {
                            Nome = Faker.Name.FullName(),
                            DataNascimento = Faker.Date.Past(50, DateTime.Now.AddYears(-25))
                        }
                    }
                }
            });
        }
        return consultas;
    }

    public IEnumerable<Consulta> CreateListConsultas(int count)
    {
        var consultas = new List<Consulta>();
        for (int i = 0; i < count; i++)
        {
            var dto = CreateConsultaDTOValid();
            consultas.Add(ConsultaValid(dto));
        }
        return consultas;
    }

    public Consulta CreateConsultaWithSpecificDate(DateTime dataHora, bool ativo = true)
    {
        var dto = CreateConsultaDTOValid(dataHoraConsulta: dataHora);
        var consulta = ConsultaValid(dto);
        
        if (!ativo)
        {
            consulta.CancelarConsulta(MotivosCancelamentoConsulta.MOTIVOS_PESSOAIS);
        }
        
        return consulta;
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
