using API_ClinicaMedica.Tests.Application.Fixtures;
using Microsoft.SqlServer.Server;
using Moq;

namespace API_ClinicaMedica.Tests.Application.BusinessValidations.MarcarConsultaValidation;

public class DataValidator_Test : IClassFixture<ConsultaTestFixture>
{
    private readonly ConsultaTestFixture _fixture;

    public DataValidator_Test(ConsultaTestFixture fixture)
    {
        _fixture = fixture;
    }
    public static IEnumerable<object[]> DatasMaiorOuIgualTresMesesEAntecedencia =>
        new List<object[]>
        {
            new object[] { DateTime.Now.AddHours(2) },      // 2 horas no futuro da fixture (agora = 10:00)
            new object[] { new DateTime(2025, 12, 4, 10, 0, 0) },     // 3 meses - 1 dia (borda válida)
            new object[] { new DateTime(2025, 12, 5, 10, 0, 0) },     // Exatamente 3 meses
            new object[] { new DateTime(2025, 11, 15, 14, 30, 0) },   // 2 meses e meio no futuro
            
        };

    [Theory(DisplayName = "Validação deve retornar SUCESSO quando DATA for VÁLIDA")]
    [MemberData(nameof(DatasMaiorOuIgualTresMesesEAntecedencia))]
    public async Task DeveRetornarSucesso_QuandoDataForValida(DateTime dataConsulta)
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        //var dataAgora = _fixture.agora;
        var dataValida = dataConsulta; // Data válida (2 horas no futuro)
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataValida);
        // Act
        var result = await _fixture.CreateDataValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
    }


    [Fact(DisplayName = "Validação deve retornar ERRO quando DATA for MENOR que a ATUAL")]
    public async Task DeveRetornarFalha_QuandoDataForMenorQueAAtual()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var data = _fixture.agora.AddHours(-2); // Data válida (2 horas no futuro)
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: data);
        // Act
        var result = await _fixture.CreateDataValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("DataInvalidaMenorQueAtual", result.Error.Id);
        Assert.Equal("A data da consulta não pode ser menor que a data atual.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }

    [Fact(DisplayName = "Validação deve retornar ERRO quando ANTECEDÊNCIA for INVÁLIDA")]
    //[InlineData("2025-09-04T10:00:00")] // Data no passado
    public async Task DeveRetornarFalha_QuandoAntecedenciaForInvalida()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dataConsulta =
            _fixture.agora.AddMinutes(40); // Tentando marcar com 40 minutos de antecedência, quando o mínimo é 1 hora
        var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
        // Act
        var result = await _fixture.CreateDataValidator().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("AntecedenciaMinimaNecessaria", result.Error.Id);
        Assert.Equal("A consulta precisa ser marcada com pelo menos 1h de antecedência.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }

    [Fact(DisplayName = "Validação deve retornar ERRO quando DATA for MAIOR que 3 meses")]
         //[InlineData("2025-09-04T10:00:00")] // Data no passado
         public async Task DeveRetornarFalha_QuandoDataMaiorQue3Meses()
         {
             _fixture.MockUnitOfWork.Reset();
             // Arrange
             var dataConsulta = _fixture.agora.AddMonths(4); // Tentando marcar com 4 meses de antecedência, máximo é 3 meses
             var dto = _fixture.CreateConsultaDTO(dataHoraConsulta: dataConsulta);
             // Act
             var result = await _fixture.CreateDataValidator().Validacao(dto);
             // Assert
             Assert.True(result.IsFailure);
             Assert.Equal("DataInvalidaMaiorTresMeses", result.Error.Id);
             Assert.Equal("A data da consulta tem que ser menor que três meses subsequentes.", result.Error.mensagem);
             Assert.Equal(400, result.Error.StatusCode);
         }
         

}