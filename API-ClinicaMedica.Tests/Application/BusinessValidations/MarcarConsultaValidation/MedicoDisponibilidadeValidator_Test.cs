using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.BusinessValidations.MarcarConsultaValidation;

public class MedicoDisponibilidadeValidator_Test : IClassFixture<ConsultaTestFixture>
{
    private readonly ConsultaTestFixture _fixture;
    public MedicoDisponibilidadeValidator_Test(ConsultaTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    
    [Fact(DisplayName = "Validação deve retornar SUCESSO quando Médico for ESPECIFICADO e ESTIVER DISPONÍVEL")]
    public async Task Validacao_DeveRetornarSucesso_QuandoMedicoForEspecificadoEEstiverDisponivel()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var dto = _fixture.CreateConsultaDTO(1, 1, Especialidades.CARDIOLOGIA, new DateTime(2025, 9, 8, 10, 30, 0));
        var medicoId = dto.IdMedico;
        var especialidade = dto.Especialidade;
        var dataConsulta = dto.DataHoraConsulta;
        _fixture.MockUnitOfWork.Setup(u =>
            u.Consultas.VerificarDisponibilidadeConsulta(medicoId, dataConsulta))
            .ReturnsAsync(true);
        
        _fixture.MockUnitOfWork.Setup(u => u.Medicos.GetEspecialidadeById(medicoId))
            .ReturnsAsync(especialidade);
        
        // Act
        var result = await _fixture.CreateMedicoDisponibilidadeValidator().Validacao(dto);
        // Assert
        
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Consultas.VerificarDisponibilidadeConsulta(medicoId, dataConsulta), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Medicos.GetEspecialidadeById(medicoId), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando Médico for ESPECIFICADO mas NÃO está DISPONÍVEL")]
    public async Task Validacao_DeveRetornarFalha_QuandoMedicoForEspecificadoMasNaoEstaDisponivel()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var dto = _fixture.CreateConsultaDTO(1, 1, Especialidades.CARDIOLOGIA, new DateTime(2025, 9, 8, 10, 30, 0));
        var medico = _fixture.MedicoValid(dto, true);
        var medicoId = dto.IdMedico;
        var especialidade = dto.Especialidade;
        var dataConsulta = dto.DataHoraConsulta;
        _fixture.MockUnitOfWork.Setup(u =>
                u.Consultas.VerificarDisponibilidadeConsulta(medicoId, dataConsulta))
            .ReturnsAsync(false);
        
        // Act
        var result = await _fixture.CreateMedicoDisponibilidadeValidator().Validacao(dto);
        // Assert
        
        Assert.True(result.IsFailure);
        Assert.Equal("MedicoNaoDisponivelNaDataEscolhida", result.Error.Id);
        Assert.Equal("O médico não está disponível na data escolhida.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Consultas.VerificarDisponibilidadeConsulta(medicoId, dataConsulta), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando Médico for ESPECIFICADO mas com ESPECIALIDADE diferente")]
    public async Task Validacao_DeveRetornarFalha_QuandoMedicoForEspecificadoMasEspecialidadeDiferente()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var dto = _fixture.CreateConsultaDTO(1, 1, Especialidades.CARDIOLOGIA, new DateTime(2025, 9, 8, 11, 30, 0, 0, 0));
        var medicoId = dto.IdMedico;
        var especialidade = Especialidades.GINECOLOGIA;
        var dataConsulta = dto.DataHoraConsulta;
        _fixture.MockUnitOfWork.Setup(u =>
                u.Consultas.VerificarDisponibilidadeConsulta(medicoId, It.IsAny<DateTime>()))
            .ReturnsAsync(true);
        _fixture.MockUnitOfWork.Setup(u => u.Medicos.GetEspecialidadeById(medicoId))
            .ReturnsAsync(especialidade);
        
        // Act
        var result = await _fixture.CreateMedicoDisponibilidadeValidator().Validacao(dto);
        // Assert
        
        Assert.True(result.IsFailure);
        Assert.Equal("MedicoNaoPossuiEspecialidade", result.Error.Id);
        Assert.Equal("O Médico não possui a especialidade informada.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Consultas.VerificarDisponibilidadeConsulta(medicoId, dataConsulta), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Medicos.GetEspecialidadeById(medicoId), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando Médico NÃO for ESPECIFICADO e NENHUM Médico ESTIVER DISPONÍVEL")]
    public async Task Validacao_DeveRetornarFalha_QuandoNenhumMedicoForEspecificadoENenhumEstaDisponivel()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var dto = _fixture.CreateConsultaDTO(1, 1, Especialidades.CARDIOLOGIA, new DateTime(2025, 9, 8, 11, 30, 0, 0, 0));
        dto.IdMedico = 0;
        var dataConsulta = dto.DataHoraConsulta;
        var especialidade = dto.Especialidade;
        
        _fixture.MockUnitOfWork.Setup(u =>
                u.Consultas.EscolheMedicoAleatorio(It.IsAny<DateTime>(), especialidade))
            .ReturnsAsync((Medico?) null);
        
        // Act
        var result = await _fixture.CreateMedicoDisponibilidadeValidator().Validacao(dto);
        // Assert
        
        Assert.True(result.IsFailure);
        Assert.Equal("NenhumMedicoDisponivel", result.Error.Id);
        Assert.Equal("Nenhum médico disponível para a data e especialidade informadas.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Consultas.EscolheMedicoAleatorio(dataConsulta, especialidade), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar SUCESSO quando Médico NÃO for ESPECIFICADO e ACHAR um Médico DISPONÍVEL")]
    public async Task Validacao_DeveRetornarSucesso_QuandoNenhumMedicoForEspecificadoEAcharUmMedicoDisponivel()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var dto = _fixture.CreateConsultaDTO(1, 1, Especialidades.CARDIOLOGIA, new DateTime(2025, 9, 8, 11, 30, 0, 0, 0));
        dto.IdMedico = 0;
        var dataConsulta = dto.DataHoraConsulta;
        var especialidade = dto.Especialidade;
        var medico = _fixture.MedicoValid(dto, true);
        
        _fixture.MockUnitOfWork.Setup(u =>
                u.Consultas.EscolheMedicoAleatorio(It.IsAny<DateTime>(), especialidade))
            .ReturnsAsync(medico);
        
        // Act
        var result = await _fixture.CreateMedicoDisponibilidadeValidator().Validacao(dto);
        // Assert
        
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Consultas.EscolheMedicoAleatorio(dataConsulta, especialidade), Times.Once);
    }
    
}