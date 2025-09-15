
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;


namespace API_ClinicaMedica.Tests.Application.BusinessValidations.MarcarConsultaValidation;

public class HorarioFuncionamentoClinicaValidator_Test : IClassFixture<ConsultaTestFixture>
{
    private readonly ConsultaTestFixture _fixture;

    public HorarioFuncionamentoClinicaValidator_Test(ConsultaTestFixture fixture)
    {
        _fixture = fixture;
    }
    
    
    [Theory(DisplayName = "Validação deve retornar SUCESSO quando consulta for em dia útil e dentro do horário de atendimento")]
    [InlineData(05, 09, 2025, 8, 0)] // Sexta-feira - 8:00
    public async Task Validacao_DeveRetornarSucesso_QuandoConsultaForDiaUtilEHorarioAtendimentoValido(int dia, int mes, int ano, int horaConsulta, int minutoConsulta){
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        DateTime dataConsulta = new DateTime(ano, mes, dia, horaConsulta, minutoConsulta, 0, 0);
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
        // Act
        var result = await _fixture.CreateHorarioFuncionamentoClinicaValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    
    [Theory(DisplayName = "Validação deve retornar ERRO quando consulta for em dia NÃO ÚTIL")]
    [InlineData(06, 09, 2025)] // Sexta-feira
    public async Task Validacao_DeveRetornarFalha_QuandoConsultaForDiaNaoUtil(int dia, int mes, int ano){
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        DateTime dataConsulta = new DateTime(ano, mes, dia, _fixture.Faker.Random.Int(8, 18), _fixture.Faker.Random.Int(0, 60), 0, 0);
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
        // Act
        var result = await _fixture.CreateHorarioFuncionamentoClinicaValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("DataForaDosDiasUteis", result.Error.Id);
        Assert.Equal("A consulta só pode ser marcada de segunda a sexta-feira.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }

    
    [Theory(DisplayName = "Validação deve retornar ERRO quando HORÁRIO da consulta for FORA do horário de atendimento")]
    [InlineData( 7, 59)] // 07:59 - Rejeita
    [InlineData( 18, 0)] // 18:00 - Rejeita
    [InlineData( 18, 01)] // 18:01 - Rejeita
    public async Task Validacao_DeveRetornarFalha_QuandoConsultaForForaDoExpediente(int horaConsulta, int minutoConsulta){
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        DateTime dataConsulta = new DateTime(2025, 09, 05, horaConsulta, minutoConsulta, 0, 0);
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
        // Act
        var result = await _fixture.CreateHorarioFuncionamentoClinicaValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("DataForaDoHorarioDeAtendimento", result.Error.Id);
        Assert.Equal("A data da consulta precisa estar dentro do horário de atendimento (08:00 - 18:00).", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }
}