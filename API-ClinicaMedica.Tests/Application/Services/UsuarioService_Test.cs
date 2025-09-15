using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Services;

public class UsuarioService_Test : IClassFixture<UsuarioServiceFixture>
{
    private UsuarioServiceFixture _fixture;
    public UsuarioService_Test(UsuarioServiceFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact(DisplayName = "CreateUser deve retornar SUCESSO quando dados forem VÁLIDOS e não houver conflitos")]
    public async Task CreateUser_DeveRetornarSucesso_QuandoDadosForemValidosENaoHouverConflitos()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUsuarioDTOValid();
        var mapper = _fixture.CreateUniqueFieldsValidationDTO();
        var usuario = _fixture.CreateUsuarioValid();
        var senha = dto.Senha;
        
        _fixture.MockValidacaoCpf.Setup(v => v.Validacao(mapper))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockValidacaoEmail.Setup(v => v.Validacao(mapper))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockValidacaoTelefone.Setup(v => v.Validacao(mapper))
            .ReturnsAsync(Result<bool>.Success(true));
        
        usuario.HashSenha(dto.Senha);
        
        _fixture.MockMapper.Setup(v => v.Map<UniqueFieldsValidationDTO>(dto))
            .Returns(mapper);
        
        _fixture.MockMapper.Setup(v => v.Map<Usuario>(dto))
            .Returns(usuario);
        
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.AddAsync(usuario))
            .Returns(Task.CompletedTask);

        _fixture.MockUnitOfWork.Setup(u => u.CommitAsync())
            .ReturnsAsync(true);
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.CreateUser(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(string.IsNullOrEmpty(result.Value.Senha));
        Assert.NotEqual(senha, result.Value.Senha);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.AddAsync(usuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }
    
    
    [Fact(DisplayName = "GetById deve retornar SUCESSO quando o USUARIO existir no DB")]
    public async Task CreateUser_DeveRetornarSucesso_QuandoOUsuarioExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUsuarioDTOValid();
        var usuario = _fixture.CreateUsuarioValid(); // Usuario já com ID definido (1)
        var usuarioDTO = _fixture.CreateUsuarioDTOValid(usuario);

        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(usuario.IdUsuario))
            .ReturnsAsync(usuario);
        
        _fixture.MockMapper.Setup(v => v.Map<UsuarioDTO>(dto))
            .Returns(usuarioDTO);
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.GetUserById(usuario.IdUsuario);

        // Assert
        Assert.True(result.IsSuccess);
    }
    [Fact(DisplayName = "GetById deve retornar ERRO quando o USUARIO NAO existir no DB")]
    public async Task CreateUser_DeveRetornarFalha_QuandoOUsuarioNaoExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUsuarioDTOValid();
        var usuario = _fixture.CreateUsuarioValid(); // Usuario já com ID definido (1)
        var usuarioDTO = _fixture.CreateUsuarioDTOValid(usuario);

        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetUserById(usuario.IdUsuario))
            .ReturnsAsync((Usuario?)null);
        _fixture.MockMapper.Setup(v => v.Map<UsuarioDTO>(dto))
            .Returns(usuarioDTO);
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.GetUserById(usuario.IdUsuario);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("UsuarioNaoEncontrado", result.Error.Id);
        Assert.Equal("Usuário não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
    }
    
     [Fact(DisplayName = "GetAllUsers deve retornar SUCESSO quando TODOS os USUARIO forem carregados do DB")]
    public async Task GetAllUsers_DeveRetornarSucesso_QuandoTodosOsUsuariosForemCarregadosDoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUsuarioDTOValid();
        var listaUsuarios = _fixture.listaUsuarios();

        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetAllUsers())
            .ReturnsAsync(listaUsuarios);
        
        _fixture.MockMapper.Setup(v => v.Map<UsuarioDTO>(dto))
            .Returns(It.IsAny<UsuarioDTO>());
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.GetAllUsers();

        // Assert
        Assert.True(result.IsSuccess);
    }
    [Fact(DisplayName = "GetAllUsers deve retornar ERRO quando NÃO houver USUARIO no DB")]
    public async Task GetAllUsers_DeveRetornarFalha_QuandoNaoHouveremUsuariosNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dto = _fixture.CreateUsuarioDTOValid();

        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.GetAllUsers())
            .ReturnsAsync((IEnumerable<Usuario>?)null);
        
        _fixture.MockMapper.Setup(v => v.Map<UsuarioDTO>(dto))
            .Returns(It.IsAny<UsuarioDTO>());
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.GetAllUsers();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("UsuariosNaoEncontrado", result.Error.Id);
        Assert.Equal("Não foram encontrados Usuários na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
    }
    
    [Fact(DisplayName = "UpdateUser deve retornar SUCESSO quando UPDATE NÃO tiver erros nos novos dados do USUARIO")]
    public async Task UpdateUser_DeveRetornarSucesso_QuandoNaoHouveremErrosNosNovosDadosDoUsuarioNoDB()
    {
        var dto = _fixture.CreateUpdateUsuarioDTOValid();
        var mapper = _fixture.CreateUniqueFieldsValidationDTO(dto.IdUsuario);
        var usuario = _fixture.CreateUsuarioValid(); // Usuario já com ID definido (1)

        _fixture.MockUnitOfWork.Setup(v => v.Usuarios.GetUserById(dto.IdUsuario))
            .ReturnsAsync(usuario);
        
        _fixture.MockValidacaoCpf.Setup(v => v.Validacao(mapper))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockValidacaoEmail.Setup(v => v.Validacao(mapper))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockValidacaoTelefone.Setup(v => v.Validacao(mapper))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockMapper.Setup(v => v.Map<UniqueFieldsValidationDTO>(usuario))
            .Returns(mapper);
        
        _fixture.MockMapper.Setup(v => v.Map<Usuario>(dto))
            .Returns(usuario);
        
        _fixture.MockMapper.Setup(v => v.Map<UpdateUsuarioDTO, Usuario>(dto))
            .Returns(usuario);

        _fixture.MockUnitOfWork.Setup(u => u.CommitAsync())
            .ReturnsAsync(true);
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.UpdateUser(dto.IdUsuario, dto);

        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.UpdateAsync(usuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<UpdateUsuarioDTO, Usuario>(dto), Times.Once);
        
    }
    [Fact(DisplayName = "UpdateUser deve retornar ERRO quando CPF do UPDATE NÃO JÁ estiver sendo usado por outro USUARIO")]
    public async Task UpdateUser_DeveRetornarFalha_QuandoCpfJaEstiverSendoUsadoNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        var dto = _fixture.CreateUpdateUsuarioDTOValid();
        var mapperUniqueFields = _fixture.CreateUniqueFieldsValidationDTO();
        var usuarioUpdate = _fixture.CreateUpdateUsuarioDTOValid();
        var usuario = _fixture.CreateUsuarioValid(); // Usuario já com ID definido (1)

        _fixture.MockUnitOfWork.Setup(v => v.Usuarios.GetUserById(usuarioUpdate.IdUsuario))
            .ReturnsAsync(usuario);
        
        _fixture.MockValidacaoCpf.Setup(v => v.Validacao(It.IsAny<UniqueFieldsValidationDTO>()))
            .ReturnsAsync(Result.Failure(UsuarioErrosResults.CpfJaCadastrado(dto.InformacoesBasicas.Cpf)));
        
        _fixture.MockMapper.Setup(v => v.Map<UniqueFieldsValidationDTO>(usuario))
            .Returns(mapperUniqueFields);
        
        _fixture.MockMapper.Setup(v => v.Map<Usuario>(dto))
            .Returns(usuario);

        _fixture.MockUnitOfWork.Setup(u => u.CommitAsync())
            .ReturnsAsync(false);
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.UpdateUser(dto.IdUsuario, dto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("CpfJaCadastrado", result.Error.Id);
        Assert.Equal($"O CPF '{dto.InformacoesBasicas.Cpf}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(u => u.Usuarios.UpdateAsync(usuario), Times.Never);
        _fixture.MockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
    [Fact(DisplayName = "UpdateUser deve retornar ERRO quando EMAIL do UPDATE NÃO JÁ estiver sendo usado por outro USUARIO")]
    public async Task UpdateUser_DeveRetornarFalha_QuandoEmailJaEstiverSendoUsadoNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        var dto = _fixture.CreateUpdateUsuarioDTOValid();
        var mapperUniqueFields = _fixture.CreateUniqueFieldsValidationDTO(dto.IdUsuario, dto.Email, dto.InformacoesBasicas.Telefone);
        var usuarioUpdate = _fixture.CreateUpdateUsuarioDTOValid(); 
        var usuario = _fixture.CreateUsuarioValid(); // Usuario já com ID definido (1)
        var usuarioDTO = _fixture.CreateUsuarioDTOValid(usuario);

        _fixture.MockUnitOfWork.Setup(v => v.Usuarios.GetUserById(usuarioUpdate.IdUsuario))
            .ReturnsAsync(usuario);
        
        _fixture.MockValidacaoEmail.Setup(v => v.Validacao(It.IsAny<UniqueFieldsValidationDTO>()))
            .ReturnsAsync(Result<bool>.Failure(UsuarioErrosResults.EmailJaCadastrado(dto.Email)));
        
        _fixture.MockValidacaoCpf.Setup(v => v.Validacao(It.IsAny<UniqueFieldsValidationDTO>()))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockValidacaoTelefone.Setup(v => v.Validacao(It.IsAny<UniqueFieldsValidationDTO>()))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockMapper.Setup(v => v.Map<UniqueFieldsValidationDTO>(usuario))
            .Returns(mapperUniqueFields);
        
        _fixture.MockMapper.Setup(v => v.Map<Usuario>(dto))
            .Returns(usuario);
        
        _fixture.MockMapper.Setup(v => v.Map<UsuarioDTO>(usuario))
            .Returns(usuarioDTO);

        _fixture.MockUnitOfWork.Setup(u => u.CommitAsync())
            .ReturnsAsync(false);
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.UpdateUser(dto.IdUsuario, dto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("EmailJaCadastrado", result.Error.Id);
        Assert.Equal($"O email '{dto.Email}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
    }
    [Fact(DisplayName = "UpdateUser deve retornar ERRO quando TELEFONE do UPDATE NÃO JÁ estiver sendo usado por outro USUARIO")]
    public async Task UpdateUser_DeveRetornarFalha_QuandoTelefoneJaEstiverSendoUsadoNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        var dto = _fixture.CreateUpdateUsuarioDTOValid();
        var mapperUniqueFields = _fixture.CreateUniqueFieldsValidationDTO(dto.IdUsuario, dto.Email, dto.InformacoesBasicas.Telefone);
        var usuarioUpdate = _fixture.CreateUpdateUsuarioDTOValid(); 
        var usuario = _fixture.CreateUsuarioValid(); // Usuario já com ID definido (1)
        var usuarioDTO = _fixture.CreateUsuarioDTOValid(usuario);

        _fixture.MockUnitOfWork.Setup(v => v.Usuarios.GetUserById(usuarioUpdate.IdUsuario))
            .ReturnsAsync(usuario);
        
        _fixture.MockValidacaoEmail.Setup(v => v.Validacao(It.IsAny<UniqueFieldsValidationDTO>()))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockValidacaoCpf.Setup(v => v.Validacao(It.IsAny<UniqueFieldsValidationDTO>()))
            .ReturnsAsync(Result<bool>.Success(true));
        
        _fixture.MockValidacaoTelefone.Setup(v => v.Validacao(It.IsAny<UniqueFieldsValidationDTO>()))
            .ReturnsAsync(Result<bool>.Failure(UsuarioErrosResults.TelefoneJaCadastrado(dto.InformacoesBasicas.Telefone)));
        
        _fixture.MockMapper.Setup(v => v.Map<UniqueFieldsValidationDTO>(usuario))
            .Returns(mapperUniqueFields);
        
        _fixture.MockMapper.Setup(v => v.Map<Usuario>(dto))
            .Returns(usuario);
        
        _fixture.MockMapper.Setup(v => v.Map<UsuarioDTO>(usuario))
            .Returns(usuarioDTO);
        
        _fixture.MockUnitOfWork.Setup(u => u.Usuarios.UpdateAsync(It.IsAny<Usuario>()))
            .Returns(Task.CompletedTask);

        _fixture.MockMapper.Setup(v => v.Map<UpdateUsuarioDTO, Usuario>(dto))
            .Returns(usuario);

        _fixture.MockUnitOfWork.Setup(u => u.CommitAsync())
            .ReturnsAsync(false);
        
        
        
        var service = _fixture.CreateUsuarioService();
        // Act
        var result = await service.UpdateUser(dto.IdUsuario, dto);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("TelefoneJaCadastrado", result.Error.Id);
        Assert.Equal($"O telefone '{dto.InformacoesBasicas.Telefone}' já está cadastrado na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockMapper.Verify(m => m.Map<UpdateUsuarioDTO, Usuario>(dto), Times.Once);
    }
     
    
}