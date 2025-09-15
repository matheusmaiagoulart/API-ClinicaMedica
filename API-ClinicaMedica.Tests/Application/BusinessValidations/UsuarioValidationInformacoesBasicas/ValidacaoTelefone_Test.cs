using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.BusinessValidations.UsuarioValidationInformacoesBasicas;

public class ValidacaoTelefone_Test : IClassFixture<ValidacaoInformacoesBasicasFixture>
{
    private readonly ValidacaoInformacoesBasicasFixture _fixture;
    public ValidacaoTelefone_Test(ValidacaoInformacoesBasicasFixture fixture)
    {
        _fixture = fixture;
    }
    
    //Validação ao cadastrar novo usuário
    [Fact(DisplayName = "Validação deve retornar SUCESSO quando TELEFONE NÃO EXISITR na base ao cadastrar NOVO usuário")]
    public async Task Validacao_DeveRetornarSucesso_QuandoTelefoneForUnico_AoCadastrarNovoUsuario()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 0);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isTelefoneAvailable(It.IsAny<String>()))
            .ReturnsAsync(true);
        // Act
        var result = await _fixture.CreateValidacaoTelefone().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando TELEFONE de cadastro JÁ está sendo USADO")]
    public async Task Validacao_DeveRetornarFalha_QuandoTelefoneJaEstaSendoUsado_AoCadastrarNovoUsuario()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 0);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone))
            .ReturnsAsync(false);
        // Act
        var result = await _fixture.CreateValidacaoTelefone().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("TelefoneJaCadastrado", result.Error.Id);
        Assert.Equal($"O telefone '{dto.InformacoesBasicas.Telefone}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone), Times.Once);
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
        var result = await _fixture.CreateValidacaoTelefone().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("UsuarioNaoEncontrado", result.Error.Id);
        Assert.Equal("Usuário não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando TELEFONE para UPDATE for passado mas JÁ existir na base de dados")]
    public async Task Validacao_DeveRetornarFalha_QuandoTelefoneParaUpdateForPassado_MasJaEstaSendoUsado()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1);
        var usuario = _fixture.CreateUsuario(dto.IdUsuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync(usuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone))
            .ReturnsAsync(false);
        // Act
        var result = await _fixture.CreateValidacaoTelefone().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("TelefoneJaCadastrado", result.Error.Id);
        Assert.Equal($"O telefone '{dto.InformacoesBasicas.Telefone}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar SUCESSO quando TELEFONE para UPDATE for passado mas NÃO existir na base de dados")]
    public async Task Validacao_DeveRetornarSucesso_QuandoEmailParaUpdateForPassado_MasNaoSendoUsado()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1);
        var usuario = _fixture.CreateUsuario(dto.IdUsuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync(usuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone))
            .ReturnsAsync(true);
        // Act
        var result = await _fixture.CreateValidacaoTelefone().Validacao(dto);
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone), Times.Once);
    }
    
    [Fact(DisplayName = "Validação deve retornar ERRO quando TELEFONE para UPDATE for passado e for IGUAL o do USUÁRIO que tenta alterar")]
    public async Task Validacao_DeveRetornarFalha_QuandoEmailParaUpdateForPassado_MasForIgualAoUsuarioQueTentaAlterar()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUniqueFieldsValidationDTO(idUsuario: 1, "11900000000");
        var usuario = _fixture.CreateUsuario(dto.IdUsuario, "11900000000");
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync(usuario);
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone))
            .ReturnsAsync(false);
        // Act
        var result = await _fixture.CreateValidacaoTelefone().Validacao(dto);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("TelefoneJaCadastrado", result.Error.Id);
        Assert.Equal($"O telefone '{dto.InformacoesBasicas.Telefone}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.GetUserById(dto.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.isTelefoneAvailable(dto.InformacoesBasicas.Telefone), Times.Once);
    }
}