using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.BusinessValidations.MarcarConsultaValidation;

public class ValidarHoraConsulta_Test : IClassFixture<ConsultaTestFixture>
{
    private readonly ConsultaTestFixture _fixture;

    public ValidarHoraConsulta_Test(ConsultaTestFixture fixture)
    {
        _fixture = fixture;
    }
    [Theory(DisplayName = "Validação deve retornar SUCESSO quando HORA da CONSULTA for VÁLIDA (08:00, 08:30, etc...)")]
    [InlineData("2025-09-05T08:00:00")] // Hora válida (08:00)
    [InlineData("2025-09-05T17:30:00")] // Hora válida (17:30)
    public async Task DeveRetornarSucesso_QuandoHoraDaConsultaForValida(DateTime horaConsulta)
    // Horários válidos são: 08:00, 08:30, 09:00, etc... até 17:30 (último horário do dia)
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dataConsulta = horaConsulta; // Data com hora válida
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
        // Act
        var result = await _fixture.CreateValidarHoraConsulta().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Theory(DisplayName = "Validação deve retornar ERRO quando HORA da CONSULTA for INVÁLIDO (08:20, 08:07, etc...)")]
    [InlineData("2025-09-05T08:02:00")] // Hora inválida (08:02)
    [InlineData("2025-09-05T08:00:01")] // Hora inválida (08:02)
    public async Task DeveRetornarFalha_QuandoHoraDaConsultaForInvalido(DateTime horaConsulta)
        // Horários válidos são: 08:00, 08:30, 09:00, etc... até 17:30 (último horário do dia)
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dataConsulta = horaConsulta; // Data com hora válida
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
        // Act
        var result = await _fixture.CreateValidarHoraConsulta().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("HorarioNaoPermitido", result.Error.Id);
        Assert.Contains("08:30", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }
    
    
    [Theory(DisplayName = "Validação deve retornar ERRO quando NÃO houver mais consultas disponíveis no dia")]
    [InlineData("2025-09-05T19:00:00")] // Hora válida (08:00)
    public async Task DeveRetornarFalha_QuandoNaoHouverMaisConsultasDisponiveisNoDia(DateTime horaConsulta)
        // Horários válidos são: 08:00, 08:30, 09:00, etc... até 17:30 (último horário do dia)
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dataConsulta = horaConsulta;
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
        // Act
        var result = await _fixture.CreateValidarHoraConsulta().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("HorarioNaoPermitido", result.Error.Id);
        Assert.Contains("Esse horário não é permitido! O Próximo horário é:", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando a próxima hora disponível for 00:00 (início do próximo dia)")]
    public async Task DeveRetornarFalha_QuandoProximaConsultaFor00h()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dataConsulta = new DateTime(2025, 9, 8, 21, 0, 0);
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
        // Act
        var result = await _fixture.CreateValidarHoraConsulta().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("HorarioNaoPermitido", result.Error.Id);
        Assert.Contains("08:00", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }

}