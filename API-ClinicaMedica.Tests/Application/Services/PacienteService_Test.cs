using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.ValueObjects;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Microsoft.Identity.Client;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Services;

public class PacienteService_Test : IClassFixture<PacienteServiceFixture>
{
    private PacienteServiceFixture _fixture;
    public PacienteService_Test(PacienteServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "CreatePaciente Deve Retornar Sucesso Quando O Usuario Existir E Nao Estiver Vinculado")]
    public async Task CreatePaciente_DeveRetornarSucesso_QuandoOUsuarionNaoExistirENaoEstiverVinculado()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var CreatePacienteDTO = _fixture.CreatePacienteDTOValid(1);
        var usuario = _fixture.CreateUsuarioValid();
        var paciente = _fixture.CreatePacienteValid(usuario.IdUsuario);

        _fixture.MockUnitOfWork.Setup(v => v.Usuarios.existsById(CreatePacienteDTO.IdUsuario))
            .ReturnsAsync(true);
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.existsById(CreatePacienteDTO.IdUsuario))
            .ReturnsAsync(false);

        _fixture.MockMapper.Setup(v => v.Map<Paciente>(CreatePacienteDTO))
            .Returns(paciente);


        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.CreatePaciente(CreatePacienteDTO);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(usuario.IdUsuario, result.Value.IdPaciente);
        Assert.True(result.Value.Ativo);
        _fixture.MockUnitOfWork.Verify(v => v.Pacientes.AddAsync(paciente), Times.Once);
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Once);

    }
    
    [Fact(DisplayName = "CreatePaciente Deve Retornar ERRO Quando O Usuario NÃO existir no BD")]
    public async Task CreatePaciente_DeveRetornarFalha_QuandoOUsuarioNaoExistir()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var CreatePacienteDTO = _fixture.CreatePacienteDTOValid(1);
        var usuario = _fixture.CreateUsuarioValid();
        var paciente = _fixture.CreatePacienteValid(usuario.IdUsuario);

        _fixture.MockUnitOfWork.Setup(v => v.Usuarios.existsById(CreatePacienteDTO.IdUsuario))
            .ReturnsAsync(false);

        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.CreatePaciente(CreatePacienteDTO);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("UsuarioNaoEncontrado", result.Error.Id);
        Assert.Equal("Usuário não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(v => v.Pacientes.AddAsync(paciente), Times.Never);
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Never);

    }
    [Fact(DisplayName = "CreatePaciente Deve Retornar ERRO Quando O Usuario existir mas Já ESTIVER vinculado a outro Paciente no BD")]
    public async Task CreatePaciente_DeveRetornarFalha_QuandoOUsuarioExistirMasJaEstiverVinculado()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var CreatePacienteDTO = _fixture.CreatePacienteDTOValid(1);
        var usuario = _fixture.CreateUsuarioValid();
        var paciente = _fixture.CreatePacienteValid(usuario.IdUsuario);

        _fixture.MockUnitOfWork.Setup(v => v.Usuarios.existsById(CreatePacienteDTO.IdUsuario))
            .ReturnsAsync(true);
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.existsById(CreatePacienteDTO.IdUsuario))
            .ReturnsAsync(true);

        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.CreatePaciente(CreatePacienteDTO);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("IdJaVinculadoAUsuario", result.Error.Id);
        Assert.Equal("O Id do usuário já está vinculado a um paciente na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(v => v.Pacientes.AddAsync(paciente), Times.Never);
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Never);

    }
    
    
    [Fact(DisplayName = "GetPacienteById Deve Retornar SUCESSO quando o Paciente existir mas Já ESTIVER vinculado a outro Paciente no BD")]
    public async Task GetPacienteById_DeveRetornarSucesso_QuandoOPacienteForCarregadoDoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var paciente = _fixture.CreatePacienteValid(1);
        var pacienteDTO = _fixture.PacienteDTOValid(paciente.IdPaciente, pcd:false, ativo:true );
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetPacienteById(paciente.IdPaciente))
            .ReturnsAsync(paciente);

        _fixture.MockMapper.Setup(v => v.Map<PacienteDTO>(paciente))
            .Returns(pacienteDTO);
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.GetPacienteById(paciente.IdPaciente);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(paciente.IdPaciente, result.Value.IdPaciente);
        Assert.True(result.Value.Ativo);
    }
    [Fact(DisplayName = "GetPacienteById Deve Retornar ERRO quando o Paciente NÃO EXISTIR no BD")]
    public async Task GetPacienteById_DeveRetornarFalha_QuandoOPacienteNaoExistirNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var paciente = _fixture.CreatePacienteValid(1);
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetPacienteById(paciente.IdPaciente))
            .ReturnsAsync((Paciente?) null);
        
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.GetPacienteById(paciente.IdPaciente);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PacienteNaoEncontrado", result.Error.Id);
        Assert.Equal("Paciente não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);


    }
    
    [Fact(DisplayName = "GetAllPacientes Deve Retornar SUCESSO quando EXISTIR Pacientes no BD")]
    public async Task GetAllPacientes_DeveRetornarSucesso_QuandoExistirPacientesNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var pacientes = _fixture.GerarPacientesDTO(3);
        
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetAllPacientes())
            .ReturnsAsync(pacientes);

        _fixture.MockMapper.Setup(v => v.Map<IEnumerable<PacienteDTO>>(pacientes))
            .Returns(It.IsAny<IEnumerable<PacienteDTO>>());
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.GetAllPacientes();
        // Assert
        Assert.True(result.IsSuccess);
        
    }
    [Fact(DisplayName = "GetAllPacientes Deve Retornar ERRO quando NÃO EXISTIR Pacientes no BD")]
    public async Task GetAllPacientes_DeveRetornarFalha_QuandoNaoExistirPacientesNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var pacientes = _fixture.GerarPacientesDTO(3);
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetAllPacientes())
            .ReturnsAsync((IEnumerable<Paciente>?) null);
        
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.GetAllPacientes();
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PacientesNaoEncontrados", result.Error.Id);
        Assert.Equal("Nenhum paciente encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
    }
    
    [Fact(DisplayName = "SoftDeletePaciente Deve Retornar SUCESSO quando Paciente for DESATIVADO no BD")]
    public async Task SoftDeletePaciente_DeveRetornarSucesso_QuandoPacienteForDesativadoNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var paciente = _fixture.CreatePacienteValid(1, ativo:true);
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetPacienteById(paciente.IdPaciente))
            .ReturnsAsync(paciente);
        
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.SoftDeletePaciente(paciente.IdPaciente);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(paciente.Ativo);
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Once);
    }
    [Fact(DisplayName = "SoftDeletePaciente Deve Retornar ERRO quando Paciente JA ESTIVER DESATIVADO no BD")]
    public async Task SoftDeletePaciente_DeveRetornarFalha_QuandoPacienteJaEstiverDesativadoNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange

        var paciente = _fixture.CreatePacienteValid(1);
        paciente.Desativar();
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetPacienteById(paciente.IdPaciente))
            .ReturnsAsync(paciente);
        
        
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.SoftDeletePaciente(paciente.IdPaciente);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PacientejaInativo", result.Error.Id);
        Assert.Equal("O paciente já está inativo.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Never);
    }
    [Fact(DisplayName = "SoftDeletePaciente Deve Retornar ERRO quando Paciente NÃO for ENCONTRADO no BD")]
    public async Task SoftDeletePaciente_DeveRetornarFalha_QuandoPacienteNaoForEncontradoNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var paciente = _fixture.CreatePacienteValid(1, ativo:false);
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetPacienteById(paciente.IdPaciente))
            .ReturnsAsync((Paciente?) null);
        
        
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.SoftDeletePaciente(paciente.IdPaciente);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PacienteNaoEncontrado", result.Error.Id);
        Assert.Equal("Paciente não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Never);
    }
    
    [Fact(DisplayName = "UpdatePaciente Deve Retornar SUCESSO quando os novos dados forem válidos e o Paciente existir e Ativo no BD")]
    public async Task UpdatePaciente_DeveRetornarSucesso_QuandoOsDadosForemValidosEPacienteExistirEAtivoNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        
        var paciente = _fixture.CreatePacienteValid(1, ativo:true);
        var updatePacienteDTO = _fixture.CreateUpdatePacienteDTOValid(paciente.IdPaciente, paciente.Pcd, new MedicamentoControlado("teste", "teste", "teste", ""));
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetPacienteById(paciente.IdPaciente))
            .ReturnsAsync(paciente);
        
        _fixture.MockMapper.Setup(v => v.Map(updatePacienteDTO, paciente))
            .Returns(paciente);
        
        _fixture.MockMapper.Setup(v => v.Map<PacienteDTO>(paciente))
            .Returns(_fixture.PacienteDTOValid(paciente.IdPaciente, pcd: paciente.Pcd, ativo: paciente.Ativo, medicamentoControlado: paciente.MedicamentosControlados));
        
        
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.UpdatePaciente(updatePacienteDTO.IdPaciente, updatePacienteDTO);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(updatePacienteDTO.IdPaciente, result.Value.IdPaciente);
        Assert.Equal(updatePacienteDTO.Pcd, result.Value.Pcd);
        Assert.True(result.Value.Ativo);
        Assert.Equal(updatePacienteDTO.MedicamentosControlados, result.Value.MedicamentosControlados);
        
        _fixture.MockMapper.Verify(v => v.Map(updatePacienteDTO, paciente), Times.Once);
        _fixture.MockMapper.Verify(v => v.Map<PacienteDTO>(paciente), Times.Once);
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Once);
    }
    [Fact(DisplayName = "UpdatePaciente Deve Retornar ERRO quando Paciente NÃO existir no BD")]
    public async Task UpdatePaciente_DeveRetornarFalha_QuandoPacienteNaoExistirNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        
        // Arrange
        var updatePacienteDTO = _fixture.CreateUpdatePacienteDTOValid(1, false, new MedicamentoControlado("teste", "teste", "teste", ""));
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetPacienteById(updatePacienteDTO.IdPaciente))
            .ReturnsAsync((Paciente?) null);
        
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.UpdatePaciente(updatePacienteDTO.IdPaciente, updatePacienteDTO);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PacienteNaoEncontrado", result.Error.Id);
        Assert.Equal("Paciente não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Never);
    }
    [Fact(DisplayName = "UpdatePaciente Deve Retornar ERRO quando Paciente existir MAS estiver DESATIVADO no BD")]
    public async Task UpdatePaciente_DeveRetornarFalha_QuandoPacienteExistirMasEstiverDesativadoNoBD()
    {
        _fixture.MockUnitOfWork.Reset();
        
        // Arrange
        var paciente = _fixture.CreatePacienteValid(1);
        paciente.Desativar();
        var updatePacienteDTO = _fixture.CreateUpdatePacienteDTOValid(1, false, new MedicamentoControlado("teste", "teste", "teste", ""));
        
        _fixture.MockUnitOfWork.Setup(v => v.Pacientes.GetPacienteById(updatePacienteDTO.IdPaciente))
            .ReturnsAsync(paciente);
        
        
        // Act
        var service = _fixture.CreatePacienteService();
        var result = await service.UpdatePaciente(updatePacienteDTO.IdPaciente, updatePacienteDTO);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("PacienteInativo", result.Error.Id);
        Assert.Equal("O paciente está inativo.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(v => v.CommitAsync(), Times.Never);
        _fixture.MockUnitOfWork.Verify(v => v.Pacientes.AddAsync(paciente), Times.Never);
    }
}