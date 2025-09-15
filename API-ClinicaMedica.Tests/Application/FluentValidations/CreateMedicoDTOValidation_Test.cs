using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Tests.Application.Fixtures;

namespace API_ClinicaMedica.Tests.Application.FluentValidations;

public class CreateMedicoDTOValidation_Test : IClassFixture<FluentValidationTestFixture>
{
    private readonly FluentValidationTestFixture _fixture;

    public CreateMedicoDTOValidation_Test(FluentValidationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Válido Quando Todos Os Campos Estiverem Corretos")]
    public void CreateMedicoDTOValidation_DeveRetornarValido_QuandoTodosOsCamposEstiveremCorretos()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid();

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Inválido Quando IdUsuario For Zero")]
    public void CreateMedicoDTOValidation_DeveRetornarInvalido_QuandoIdUsuarioForZero()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(idUsuario: 0);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdUsuario");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "IdUsuario deve ser maior que zero.");
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Inválido Quando IdUsuario For Negativo")]
    public void CreateMedicoDTOValidation_DeveRetornarInvalido_QuandoIdUsuarioForNegativo()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(idUsuario: -1);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdUsuario");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "IdUsuario deve ser maior que zero.");
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Inválido Quando Especialidade For Inválida")]
    public void CreateMedicoDTOValidation_DeveRetornarInvalido_QuandoEspecialidadeForInvalida()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(especialidade: "EspecialidadeInvalida");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Especialidade");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Especialidade inválida.");
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Válido Quando Especialidade For Válida")]
    public void CreateMedicoDTOValidation_DeveRetornarValido_QuandoEspecialidadeForValida()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(especialidade: Especialidades.CARDIOLOGIA.ToString());

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Inválido Quando UfCrm For Nula")]
    public void CreateMedicoDTOValidation_DeveRetornarInvalido_QuandoUfCrmForNula()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOWithNullValues(ufCrm: null);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UfCrm");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "UF do CRM deve ser informada.");
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Inválido Quando UfCrm For Vazia")]
    public void CreateMedicoDTOValidation_DeveRetornarInvalido_QuandoUfCrmForVazia()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(ufCrm: "");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UfCrm");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "UF do CRM não pode ser vazia.");
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Inválido Quando UfCrm For Estado Inválido")]
    public void CreateMedicoDTOValidation_DeveRetornarInvalido_QuandoUfCrmForEstadoInvalido()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(ufCrm: "EstadoInvalido");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "UfCrm");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Estado inválido.");
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Válido Quando UfCrm For Estado Válido")]
    public void CreateMedicoDTOValidation_DeveRetornarValido_QuandoUfCrmForEstadoValido()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(ufCrm: Estados.SP.ToString());

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Inválido Quando CrmNumber Contiver Letras")]
    public void CreateMedicoDTOValidation_DeveRetornarInvalido_QuandoCrmNumberContierLetras()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(crmNumber: "123ABC");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CrmNumber");
        Assert.Contains(result.Errors, e => e.ErrorMessage == "O CRM do Médico deve conter apenas números.");
    }

    [Fact(DisplayName = "CreateMedicoDTOValidation Deve Retornar Válido Quando CrmNumber Contiver Apenas Números")]
    public void CreateMedicoDTOValidation_DeveRetornarValido_QuandoCrmNumberContiverApenasNumeros()
    {
        // Arrange
        var validator = _fixture.CreateMedicoDTOValidation();
        var dto = _fixture.CreateMedicoDTOValid(crmNumber: "123456");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
