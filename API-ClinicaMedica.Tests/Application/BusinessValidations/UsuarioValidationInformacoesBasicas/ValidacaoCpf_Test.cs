using System.Security.Cryptography;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.BusinessValidations.UsuarioValidationInformacoesBasicas;

public class ValidacaoCpf_Test : IClassFixture<ValidacaoInformacoesBasicasFixture>
{
    private readonly ValidacaoInformacoesBasicasFixture _fixture;
    public ValidacaoCpf_Test(ValidacaoInformacoesBasicasFixture fixture)
    {
        _fixture = fixture;
    }
    //Validação ao cadastrar novo usuário
    [Fact(DisplayName = "Validação deve retornar SUCESSO quando CPF NÃO EXISTIR na base ao cadastrar NOVO usuário")]
    public async Task Validacao_DeveRetornarSucesso_QuandoCpfForUnico_AoCadastrarNovoUsuario()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 0);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isCpfAvailable(dto.InformacoesBasicas.Cpf))
            .ReturnsAsync(true);
        // Act
        var result = await _fixture.CreateValidacaoCpf().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isCpfAvailable(dto.InformacoesBasicas.Cpf), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando CPF JÁ existir ao cadastrar NOVO usuário")]
    public async Task Validacao_DeveRetornarFalha_QuandoCpfJaExistir_AoCadastrarNovoUsuario()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 0);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isCpfAvailable(dto.InformacoesBasicas.Cpf))
            .ReturnsAsync(false);
        // Act
        var result = await _fixture.CreateValidacaoCpf().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CpfJaCadastrado", result.Error.Id);
        Assert.Equal($"O CPF '{dto.InformacoesBasicas.Cpf}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isCpfAvailable(dto.InformacoesBasicas.Cpf), Times.Once);
    }
    
    //Validação ao atualizar usuário
    [Fact(DisplayName = "Validação deve retornar ERRO quando ID for passado mas NÃO existe na base de dados")]
    public async Task Validacao_DeveRetornarFalha_QuandoIdForPassado_MasNaoExistirNoBanco()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync((Usuario?)null);
        // Act
        var result = await _fixture.CreateValidacaoCpf().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("UsuarioNaoEncontrado", result.Error.Id);
        Assert.Equal("Usuário não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando ID for passado mas CPF para UPDATE já existir na base de dados")]
    public async Task Validacao_DeveRetornarFalha_QuandoCpfParaUpdateForPassado_MasJaEstaSendoUsado()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1);
        dto.InformacoesBasicas.Cpf = "12345678901";
        var usuario = _fixture.CreateUsuario(dto.IdUsuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync(usuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isCpfAvailable(dto.InformacoesBasicas.Cpf))
            .ReturnsAsync(false);
        // Act
        var result = await _fixture.CreateValidacaoCpf().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CpfJaCadastrado", result.Error.Id);
        Assert.Equal($"O CPF '{dto.InformacoesBasicas.Cpf}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isCpfAvailable(dto.InformacoesBasicas.Cpf), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar SUCESSO quando ID for passado e o CPF para UPDATE NÃO existir na base de dados")]
    public async Task Validacao_DeveRetornarFalha_QuandoCpfParaUpdateForPassado_ENaoEstaSendoUsado()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1);
        dto.InformacoesBasicas.Cpf = "12345678901";
        var usuario = _fixture.CreateUsuario(dto.IdUsuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync(usuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isCpfAvailable(dto.InformacoesBasicas.Cpf))
            .ReturnsAsync(true);
        // Act
        var result = await _fixture.CreateValidacaoCpf().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isCpfAvailable(dto.InformacoesBasicas.Cpf), Times.Once);
    }
}