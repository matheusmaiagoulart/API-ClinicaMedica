using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.BusinessValidations.UsuarioValidationInformacoesBasicas;

public class ValidacaoEmail_Test : IClassFixture<ValidacaoInformacoesBasicasFixture>
{
    public readonly ValidacaoInformacoesBasicasFixture _fixture;
    public ValidacaoEmail_Test(ValidacaoInformacoesBasicasFixture fixture)
    {
        _fixture = fixture;
    }
    
    //Validação ao cadastrar novo usuário
    [Fact(DisplayName = "Validação deve retornar SUCESSO quando EMAIL NÃO EXISITR na base ao cadastrar NOVO usuário")]
    public async Task Validacao_DeveRetornarSucesso_QuandoEmailForUnico_AoCadastrarNovoUsuario()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 0);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isEmailAvailable(dto.Email))
            .ReturnsAsync(true);
        // Act
        var result = await _fixture.CreateValidacaoEmail().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isEmailAvailable(dto.Email), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando EMAIL de cadastro JÁ está sendo USADO")]
    public async Task Validacao_DeveRetornarFalha_QuandoEmailJaEstaSendoUsado_AoCadastrarNovoUsuario()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 0);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isEmailAvailable(dto.Email))
            .ReturnsAsync(false);
        // Act
        var result = await _fixture.CreateValidacaoEmail().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("EmailJaCadastrado", result.Error.Id);
        Assert.Equal($"O email '{dto.Email}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isEmailAvailable(dto.Email), Times.Once);
    }

    //Validação ao atualizar usuário
    [Fact(DisplayName = "Validação deve retornar ERRO quando ID for passado mas USER NÃO existir na base de dados")]
    public async Task Validacao_DeveRetornarFalha_QuandoIdForPassado_MasUserNaoExistirNoBanco()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync((Usuario?)null);
        // Act
        var result = await _fixture.CreateValidacaoEmail().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("UsuarioNaoEncontrado", result.Error.Id);
        Assert.Equal("Usuário não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando EMAIL para UPDATE for passado mas JÁ existir na base de dados")]
    public async Task Validacao_DeveRetornarFalha_QuandoEmailParaUpdateForPassado_MasJaEstaSendoUsado()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1);
        var usuario = _fixture.CreateUsuario(dto.IdUsuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync(usuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isEmailAvailable(dto.Email))
            .ReturnsAsync(false);
        // Act
        var result = await _fixture.CreateValidacaoEmail().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("EmailJaCadastrado", result.Error.Id);
        Assert.Equal($"O email '{dto.Email}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isEmailAvailable(dto.Email), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar SUCESSO quando EMAIL para UPDATE for passado mas NÃO existir na base de dados")]
    public async Task Validacao_DeveRetornarFalha_QuandoEmailParaUpdateForPassado_MasNaoSendoUsado()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1);
        var usuario = _fixture.CreateUsuario(dto.IdUsuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync(usuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isEmailAvailable(dto.Email))
            .ReturnsAsync(true);
        // Act
        var result = await _fixture.CreateValidacaoEmail().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isEmailAvailable(dto.Email), Times.Once);
    }
    
    
}