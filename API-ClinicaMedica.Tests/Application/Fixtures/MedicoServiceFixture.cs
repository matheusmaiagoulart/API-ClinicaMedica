using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Application.Services;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Infra.Interfaces;
using AutoMapper;
using Bogus;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Fixtures;

public class MedicoServiceFixture : IDisposable
{
    public Mock<IUnitOfWork> MockUnitOfWork { get; }
    public Faker Faker { get; }
    
    public Mock<IMapper> MockMapper { get; set; }
    
    public MedicoServiceFixture()
    {
        MockUnitOfWork = new Mock<IUnitOfWork>();
        Faker = new Faker();
        MockMapper = new Mock<IMapper>();
    }

    public MedicoService CreateMedicoService()
    {
        return new MedicoService(MockUnitOfWork.Object, MockMapper.Object);
    }
    
    public CreateMedicoDTO CreateMedicoDTOValid(int? idUsuario = null,string? especialidade = null,  string? crmNumber = null, string? UfCrm = null, bool? ativo = null)
    {
        return new CreateMedicoDTO
        {
            IdUsuario = idUsuario ?? Faker.Random.Int(1, 1000),
            Especialidade = especialidade ?? Especialidades.CARDIOLOGIA.ToString(),
            CrmNumber = crmNumber ?? Faker.Random.Replace("#####"),
            UfCrm = UfCrm ?? Estados.SP.ToString(),
            Ativo = ativo ?? true
        };
    }
    
    public Medico MedicoValid(CreateMedicoDTO dto)
    {
        return new Medico(
            dto.IdUsuario, 
            (Especialidades)Enum.Parse(typeof(Especialidades), dto.Especialidade), 
            dto.CrmNumber, 
            dto.Ativo, 
            (Estados)Enum.Parse(typeof(Estados), dto.UfCrm)
        );
    }

    public MedicoDTO CreateMedicoDTO(Medico dto)
    {
        return new MedicoDTO()
        {
            IdMedico = dto.IdMedico,
            Especialidade = dto.Especialidade,
            CrmNumber = dto.CrmNumber,
            UfCrm = dto.UfCrm,
            Ativo = dto.Ativo
        };
    }
    
    public IEnumerable<MedicoDTO> CreateListMedicosDTO(int count)
    {
        var medicos = new List<MedicoDTO>();
        for (int i = 0; i < count; i++)
        {
            medicos.Add(new MedicoDTO {
                
                IdMedico = Faker.Random.Int(1, 1000),
                Especialidade = Especialidades.CARDIOLOGIA, // Mockado para simplificar
                CrmNumber = Faker.Random.Replace("#####"),
                Ativo = true
            });
        }
        return medicos;
    }
    
    public IEnumerable<Medico> CreateListMedicos(int count, string? especialidade = null)
    {
        var dto = CreateMedicoDTOValid(especialidade: especialidade);
        var medicos = new List<Medico>();
        for (int i = 0; i < count; i++)
        {
            medicos.Add(MedicoValid(dto));
        }
        return medicos;
    }
    public void Dispose()
    {
        // TODO release managed resources here
    }
}