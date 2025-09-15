using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Tests.Application.Fixtures;

namespace API_ClinicaMedica.Tests.Application.FluentValidations;

public class UsuarioDTOValidator_Test : IClassFixture<FluentValidationTestFixture>
{
    private readonly FluentValidationTestFixture _fixture;

    public UsuarioDTOValidator_Test(FluentValidationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Válido Quando Todos Os Campos Estiverem Corretos")]
    public void UsuarioDTOValidator_DeveRetornarValido_QuandoTodosOsCamposEstiveremCorretos()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid();

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Email For Nulo")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoEmailForNulo()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOWithNullValues(email: null);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Email For Vazio")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoEmailForVazio()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid(email: "");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Email For Inválido")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoEmailForInvalido()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid(email: "email-invalido");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Senha For Nula")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoSenhaForNula()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOWithNullValues(senha: null);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Senha Tiver Menos De 6 Caracteres")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoSenhaTiverMenosDe6Caracteres()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid(senha: "12345");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Senha");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Nome For Nulo")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoNomeForNulo()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOWithNullValues(nome: null);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InformacoesBasicas.Nome");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando CPF For Nulo")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoCpfForNulo()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOWithNullValues(cpf: null);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InformacoesBasicas.Cpf");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando CPF Não Tiver 11 Caracteres")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoCpfNaoTiver11Caracteres()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid(cpf: "123456789");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InformacoesBasicas.Cpf");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando RG Não Tiver 9 Caracteres")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoRgNaoTiver9Caracteres()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid(rg: "1234567");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InformacoesBasicas.Rg");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Data Nascimento For Futura")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoDataNascimentoForFutura()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid(dataNascimento: DateTime.Now.AddDays(1));

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InformacoesBasicas.DataNascimento");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Telefone Não Tiver Entre 10 E 11 Caracteres")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoTelefoneNaoTiverEntre10E11Caracteres()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid(telefone: "123456789");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "InformacoesBasicas.Telefone");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando Logradouro For Nulo")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoLogradouroForNulo()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOWithNullValues(logradouro: null);

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Endereco.Logradouro");
    }

    [Fact(DisplayName = "UsuarioDTOValidator Deve Retornar Inválido Quando CEP Não Tiver 8 Caracteres")]
    public void UsuarioDTOValidator_DeveRetornarInvalido_QuandoCepNaoTiver8Caracteres()
    {
        // Arrange
        var validator = _fixture.CreateUsuarioDTOValidator();
        var dto = _fixture.CreateUsuarioDTOValid(cep: "1234567");

        // Act
        var result = validator.Validate(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Endereco.Cep");
    }
}
