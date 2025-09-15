using API_ClinicaMedica.Domain.ValueObjects;
using API_ClinicaMedica.Tests.Application.Fixtures;

namespace API_ClinicaMedica.Tests.Application.FluentValidations;

public class CreatePacienteDTOValidation_Test : IClassFixture<FluentValidationTestFixture>
{
    private readonly FluentValidationTestFixture _fixture;

    public CreatePacienteDTOValidation_Test(FluentValidationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "CreatePacienteDTOValidation Deve Retornar Válido Quando Todos Os Campos Estiverem Corretos")]
    public void CreatePacienteDTOValidation_DeveRetornarValido_QuandoTodosOsCamposEstiveremCorretos()
    {
        // Arrange
        var validator = _fixture.CreatePacienteDTOValidation();
        var dto = _fixture.CreatePacienteDTOValid();

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "CreatePacienteDTOValidation Deve Retornar Inválido Quando Nome Do Medicamento Exceder 40 Caracteres")]
    public void CreatePacienteDTOValidation_DeveRetornarInvalido_QuandoNomeDoMedicamentoExceder40Caracteres()
    {
        // Arrange
        var validator = _fixture.CreatePacienteDTOValidation();
        var medicamentosInvalidos = new List<MedicamentoControlado>
        {
            new MedicamentoControlado(
                _fixture.Faker.Random.String2(41), // Nome com 41 caracteres
                "250mg",
                "2x ao dia",
                "Observações"
            )
        };
        var dto = _fixture.CreatePacienteDTOValid(medicamentosControlados: medicamentosInvalidos);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Nome"));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "O campo 'Nome' deve ter no máximo 100 caracteres.");
    }

    [Fact(DisplayName = "CreatePacienteDTOValidation Deve Retornar Inválido Quando Dosagem Do Medicamento Exceder 40 Caracteres")]
    public void CreatePacienteDTOValidation_DeveRetornarInvalido_QuandoDosagemDoMedicamentoExceder40Caracteres()
    {
        // Arrange
        var validator = _fixture.CreatePacienteDTOValidation();
        var medicamentosInvalidos = new List<MedicamentoControlado>
        {
            new MedicamentoControlado(
                "Medicamento A",
                _fixture.Faker.Random.String2(41), // Dosagem com 41 caracteres
                "2x ao dia",
                "Observações"
            )
        };
        var dto = _fixture.CreatePacienteDTOValid(medicamentosControlados: medicamentosInvalidos);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Dosagem"));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "O campo 'Dosagem' deve ter no máximo 40 caracteres.");
    }

    [Fact(DisplayName = "CreatePacienteDTOValidation Deve Retornar Inválido Quando Frequencia Do Medicamento Exceder 40 Caracteres")]
    public void CreatePacienteDTOValidation_DeveRetornarInvalido_QuandoFrequenciaDoMedicamentoExceder40Caracteres()
    {
        // Arrange
        var validator = _fixture.CreatePacienteDTOValidation();
        var medicamentosInvalidos = new List<MedicamentoControlado>
        {
            new MedicamentoControlado(
                "Medicamento A",
                "250mg",
                _fixture.Faker.Random.String2(41), // Frequencia com 41 caracteres
                "Observações"
            )
        };
        var dto = _fixture.CreatePacienteDTOValid(medicamentosControlados: medicamentosInvalidos);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Frequencia"));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "O campo 'Frequencia' deve ter no máximo 40 caracteres.");
    }

    [Fact(DisplayName = "CreatePacienteDTOValidation Deve Retornar Inválido Quando Observacoes Do Medicamento Exceder 150 Caracteres")]
    public void CreatePacienteDTOValidation_DeveRetornarInvalido_QuandoObservacoesDoMedicamentoExceder150Caracteres()
    {
        // Arrange
        var validator = _fixture.CreatePacienteDTOValidation();
        var medicamentosInvalidos = new List<MedicamentoControlado>
        {
            new MedicamentoControlado(
                "Medicamento A",
                "250mg",
                "2x ao dia",
                _fixture.Faker.Random.String2(151) // Observações com 151 caracteres
            )
        };
        var dto = _fixture.CreatePacienteDTOValid(medicamentosControlados: medicamentosInvalidos);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Observacoes"));
        Assert.Contains(result.Errors, e => e.ErrorMessage == "O campo 'Observacoes' deve ter no máximo 150 caracteres.");
    }

    [Fact(DisplayName = "CreatePacienteDTOValidation Deve Retornar Válido Com Lista Vazia De Medicamentos")]
    public void CreatePacienteDTOValidation_DeveRetornarValido_ComListaVaziaDeMedicamentos()
    {
        // Arrange
        var validator = _fixture.CreatePacienteDTOValidation();
        var dto = _fixture.CreatePacienteDTOValid(medicamentosControlados: new List<MedicamentoControlado>());

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
