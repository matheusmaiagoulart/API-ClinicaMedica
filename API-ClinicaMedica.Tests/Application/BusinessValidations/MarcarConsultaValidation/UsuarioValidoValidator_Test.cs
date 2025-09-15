using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.BusinessValidations.MarcarConsultaValidation;


public class UsuarioValidoValidator_Test : IClassFixture<ConsultaTestFixture>
{
    private readonly ConsultaTestFixture _fixture;
    public UsuarioValidoValidator_Test(ConsultaTestFixture fixture)
    {
        _fixture = fixture;
    }

    

    [Fact (DisplayName = "Validação deve retornar SUCESSO quando Paciente EXISTIR e ATIVO")]
    public async Task Validacao_DeveRetornarSucesso_QuandoPacienteExistirEAtivo()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateConsultaDTO(1, 1);
        var id = dto.IdPaciente;
        var paciente = _fixture.PacienteValid(dto);
        _fixture.MockUnitOfWork.Setup(u => u.Pacientes.GetPacienteById(id))
            .ReturnsAsync(paciente);
        // Act
        var result = await _fixture.CreateUsuarioValidoValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Pacientes.GetPacienteById(id), Times.Once);
    }

    [Fact (DisplayName = "Validação deve retornar ERRO quando Paciente NÃO EXISTIR")]
    public async Task Validacao_DeveRetornarFalha_QuandoPacienteNaoExistir()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var id = 1;
        var dto = _fixture.CreateConsultaDTO(id, 1);
        var paciente = _fixture.PacienteValid(dto);
        paciente.Desativar(); //---------------------------------------------skjdkjdjk-------------------
        _fixture.MockUnitOfWork.Setup(u => u.Pacientes.GetPacienteById(id))
            .ReturnsAsync((Paciente?) null);
        // Act
        var result = await _fixture.CreateUsuarioValidoValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PacienteNaoEncontrado", result.Error.Id);
        Assert.Equal("Paciente não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Pacientes.GetPacienteById(id), Times.Once);
        
    }
    
    [Fact (DisplayName = "Validação deve retornar ERRO quando Paciente estiver INATIVO")]
    public async Task Validacao_DeveRetornarFalha_QuandoPacienteEstiverInativo()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var dto = _fixture.CreateConsultaDTO(1, 1);
        var id = dto.IdPaciente;
        var paciente = _fixture.PacienteValid(dto);
        paciente.Desativar(); //---------------------------------------------skjdkjdjk-------------------
        _fixture.MockUnitOfWork.Setup(u => u.Pacientes.GetPacienteById(id))
            .ReturnsAsync(paciente);
        // Act
        var result = await _fixture.CreateUsuarioValidoValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PacienteInativo", result.Error.Id);
        Assert.Equal("O paciente está inativo.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Pacientes.GetPacienteById(id), Times.Once);
        
    }





}