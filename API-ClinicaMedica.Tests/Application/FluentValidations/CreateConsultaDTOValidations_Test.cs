using API_ClinicaMedica.Tests.Application.Fixtures;

namespace API_ClinicaMedica.Tests.Application.FluentValidations;

public class CreateConsultaDTOValidations_Test : IClassFixture<FluentValidationTestFixture>
{
    private readonly FluentValidationTestFixture _fixture;

    public CreateConsultaDTOValidations_Test(FluentValidationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "CreateConsultaDTOValidations Deve Retornar Válido Quando Todos Os Campos Estiverem Corretos")]
    public void CreateConsultaDTOValidations_DeveRetornarValido_QuandoTodosOsCamposEstiveremCorretos()
    {
        // Arrange
        var validator = _fixture.CreateConsultaDTOValidations();
        var dto = _fixture.CreateConsultaDTOValid();

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "CreateConsultaDTOValidations Deve Retornar Inválido Quando IdPaciente For Zero")]
    public void CreateConsultaDTOValidations_DeveRetornarInvalido_QuandoIdPacienteForZero()
    {
        // Arrange
        var validator = _fixture.CreateConsultaDTOValidations();
        var dto = _fixture.CreateConsultaDTOValid(idPaciente: 0);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdPaciente");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "IdPaciente deve ser maior que zero.");
    }

    [Fact(DisplayName = "CreateConsultaDTOValidations Deve Retornar Inválido Quando IdPaciente For Negativo")]
    public void CreateConsultaDTOValidations_DeveRetornarInvalido_QuandoIdPacienteForNegativo()
    {
        // Arrange
        var validator = _fixture.CreateConsultaDTOValidations();
        var dto = _fixture.CreateConsultaDTOValid(idPaciente: -1);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdPaciente");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "IdPaciente deve ser maior que zero.");
    }

    [Fact(DisplayName = "CreateConsultaDTOValidations Deve Retornar Inválido Quando IdMedico For Zero")]
    public void CreateConsultaDTOValidations_DeveRetornarInvalido_QuandoIdMedicoForZero()
    {
        // Arrange
        var validator = _fixture.CreateConsultaDTOValidations();
        var dto = _fixture.CreateConsultaDTOValid(idMedico: 0);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdMedico");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "IdMedico deve ser maior que zero.");
    }

    [Fact(DisplayName = "CreateConsultaDTOValidations Deve Retornar Inválido Quando IdMedico For Negativo")]
    public void CreateConsultaDTOValidations_DeveRetornarInvalido_QuandoIdMedicoForNegativo()
    {
        // Arrange
        var validator = _fixture.CreateConsultaDTOValidations();
        var dto = _fixture.CreateConsultaDTOValid(idMedico: -1);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdMedico");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "IdMedico deve ser maior que zero.");
    }

    [Fact(DisplayName = "CreateConsultaDTOValidations Deve Retornar Válido Quando DataHoraConsulta For Informada")]
    public void CreateConsultaDTOValidations_DeveRetornarValido_QuandoDataHoraConsultaForInformada()
    {
        // Arrange
        var validator = _fixture.CreateConsultaDTOValidations();
        var dto = _fixture.CreateConsultaDTOValid(dataHoraConsulta: DateTime.Now.AddDays(1));

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
