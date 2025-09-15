using System.ComponentModel;
using API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;
using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Infra.Interfaces;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Bogus;
using Microsoft.OpenApi.Attributes;
using Moq;

namespace API_ClinicaMedica.Tests.Application.BusinessValidations.MarcarConsultaValidation;

public class MedicoValidoValidator_Test : IClassFixture<ConsultaTestFixture>
{
    private readonly ConsultaTestFixture _fixture;
    public MedicoValidoValidator_Test(ConsultaTestFixture consultaTestFixture)
    {
        _fixture = consultaTestFixture;
    }

    private Medico MedicoValido(CreateConsultaDTO consultaDTO, bool ativo)
    {
        return new Medico(consultaDTO.IdMedico, _fixture.Faker.PickRandom<Especialidades>(),
            _fixture.Faker.Random.String2(6, "0123456789"), ativo, _fixture.Faker.PickRandom<Estados>());
    }


    [Fact (DisplayName = "Validação deve retornar SUCESSO quando Medico EXISTIR e ATIVO")]
    public async Task Validacao_DeveRetornarSucesso_QuandoMedicoExistirEAtivo()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateConsultaDTO(1, 1);
        var id = dto.IdMedico;
        var medico = MedicoValido(dto, true);

        _fixture.MockUnitOfWork.Setup(m => m.Medicos.GetMedicoById(id)).ReturnsAsync(medico);
        // Act
        var result = await _fixture.CreateMedicoValidoValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Medicos.GetMedicoById(id), Times.Once);
    }
    
    [Fact (DisplayName = "Validação deve retornar ERRO quando Medico NÃO EXISTIR")]
    public async Task Validacao_DeveRetornarFalha_QuandoMedicoNaoExistir()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var id = 2;
        var dto = _fixture.CreateConsultaDTO(1, id);

        _fixture.MockUnitOfWork.Setup(m => m.Medicos.GetMedicoById(id))
            .ReturnsAsync((Medico?) null);
        // Act
        var result = await _fixture.CreateMedicoValidoValidator().Validacao(dto); 
        // Assert
        Assert.True(result.IsFailure);
        _fixture.MockUnitOfWork.Verify(u => u.Medicos.GetMedicoById(id), Times.Once);
        Assert.Equal("MedicoNaoEncontrado", result.Error.Id);
        Assert.Equal("O Id do Médico não foi encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
    }
    
    [Fact (DisplayName = "Validação deve retornar ERRO quando Medico estiver INATIVO")]
    public async Task Validacao_DeveRetornarFalha_QuandoMedicoEstiverInativo()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var id = 1;
        var dto = _fixture.CreateConsultaDTO(1, id);
        var medico = MedicoValido(dto, false);
        _fixture.MockUnitOfWork.Setup(m => m.Medicos.GetMedicoById(id))
            .ReturnsAsync(medico);
        // Act
        var result = await _fixture.CreateMedicoValidoValidator().Validacao(dto); 
        // Assert
        Assert.True(result.IsFailure);
        _fixture.MockUnitOfWork.Verify(u => u.Medicos.GetMedicoById(id), Times.Once);
        Assert.Equal("MedicoInativo", result.Error.Id);
        Assert.Equal("O Médico está inativo.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }
}