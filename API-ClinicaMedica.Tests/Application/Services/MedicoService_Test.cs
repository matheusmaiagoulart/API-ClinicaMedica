using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Services;

public class MedicoService_Test : IClassFixture<MedicoServiceFixture>
{
    private readonly MedicoServiceFixture _fixture;
    public MedicoService_Test(MedicoServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "CreateMedico deve retornar SUCESSO quando dados forem VÁLIDOS e não houver conflitos")]
    public async Task CreateMedico_DeveRetornarSucesso_QuandoDadosForemValidosENaoHouverConflitos()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dtoMedico = _fixture.CreateMedicoDTOValid(1, "CARDIOLOGIA", "12345", "SP", true);
        var medicoEntity = _fixture.MedicoValid(dtoMedico);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Usuarios.existsById(dtoMedico.IdUsuario))
            .ReturnsAsync(true);
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.existsMedicoById(dtoMedico.IdUsuario))
            .ReturnsAsync(false);
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.CRMExists(dtoMedico.CrmNumber))
            .ReturnsAsync(false);
        _fixture.MockMapper.Setup(m => m.Map<Medico>(dtoMedico)).Returns(medicoEntity);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.CreateMedico(dtoMedico);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(dtoMedico.IdUsuario, result.Value.IdMedico);
        Assert.Equal(dtoMedico.Especialidade, result.Value.Especialidade.ToString());
        Assert.Equal(dtoMedico.CrmNumber, result.Value.CrmNumber);
        Assert.Equal(dtoMedico.UfCrm, result.Value.UfCrm.ToString());
        Assert.True(result.Value.Ativo);
        _fixture.MockUnitOfWork.Verify(uow => uow.Usuarios.existsById(dtoMedico.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.existsMedicoById(dtoMedico.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.CRMExists(dtoMedico.CrmNumber), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.AddAsync(It.IsAny<Medico>()), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);

    }
    [Fact(DisplayName = "CreateMedico deve retornar ERRO quando UsuarioId NÃO existir no DB")]
    public async Task CreateMedico_DeveRetornarFalha_QuandoUsuarioIdNaoExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dtoMedico = _fixture.CreateMedicoDTOValid(1, "CARDIOLOGIA", "12345", "SP", true);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Usuarios.existsById(dtoMedico.IdUsuario))
            .ReturnsAsync(false);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.CreateMedico(dtoMedico);
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("UsuarioNaoEncontrado", result.Error.Id);
        Assert.Equal("Usuário não encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);

        _fixture.MockUnitOfWork.Verify(uow => uow.Usuarios.existsById(dtoMedico.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.AddAsync(It.IsAny<Medico>()), Times.Never);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);

    }
    [Fact(DisplayName = "CreateMedico deve retornar ERRO quando UsuarioId já estiver vinculado a um Médico")]
    public async Task CreateMedico_DeveRetornarFalha_QuandoUsuarioIdExistirEEstiverVinculadoAUmMedico()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dtoMedico = _fixture.CreateMedicoDTOValid(1, "CARDIOLOGIA", "12345", "SP", true);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Usuarios.existsById(dtoMedico.IdUsuario))
            .ReturnsAsync(true);
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.existsMedicoById(dtoMedico.IdUsuario))
            .ReturnsAsync(true); // Quer dizer que existe na tabela Medicos e já está vinculado
        
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.CreateMedico(dtoMedico);
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("IdJaVinculadoAUsuario", result.Error.Id);
        Assert.Equal("O Id do usuário já está vinculado a um Médico na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);

        _fixture.MockUnitOfWork.Verify(uow => uow.Usuarios.existsById(dtoMedico.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.existsMedicoById(dtoMedico.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.AddAsync(It.IsAny<Medico>()), Times.Never);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);

    }
    [Fact(DisplayName = "CreateMedico deve retornar ERRO quando CRM já existir no DB")]
    public async Task CreateMedico_DeveRetornarFalha_QuandoCRMJaExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dtoMedico = _fixture.CreateMedicoDTOValid(1, "CARDIOLOGIA", "12345", "SP", true);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Usuarios.existsById(dtoMedico.IdUsuario))
            .ReturnsAsync(true);
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.existsMedicoById(dtoMedico.IdUsuario))
            .ReturnsAsync(false);
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.CRMExists(dtoMedico.CrmNumber))
            .ReturnsAsync(true);
        
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.CreateMedico(dtoMedico);
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("CrmJaExistenteNoDB", result.Error.Id);
        Assert.Equal("O CRM em questão já consta cadastrado.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);

        _fixture.MockUnitOfWork.Verify(uow => uow.Usuarios.existsById(dtoMedico.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.existsMedicoById(dtoMedico.IdUsuario), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.CRMExists(dtoMedico.CrmNumber), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.AddAsync(It.IsAny<Medico>()), Times.Never);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);

    }
    
    
    [Fact(DisplayName = "GetMedicoById deve retornar SUCESSO quando Existir no DB")]
    public async Task GetMedicoById_DeveRetornarSucesso_QuandoExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dtoMedico = _fixture.CreateMedicoDTOValid(1, "CARDIOLOGIA", "12345", "SP", true);
        var medicoEntity = _fixture.MedicoValid(dtoMedico);
        var medicoDto = _fixture.CreateMedicoDTO(medicoEntity);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicoById(medicoEntity.IdMedico))
            .ReturnsAsync(medicoEntity);
        _fixture.MockMapper.Setup(m => m.Map<MedicoDTO>(medicoEntity)).Returns(medicoDto);
        
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetMedicoById(medicoEntity.IdMedico);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(dtoMedico.IdUsuario, result.Value.IdMedico);
        Assert.Equal(dtoMedico.Especialidade, result.Value.Especialidade.ToString());
        Assert.Equal(dtoMedico.CrmNumber, result.Value.CrmNumber);
        Assert.Equal(dtoMedico.UfCrm, result.Value.UfCrm.ToString());
        Assert.True(result.Value.Ativo);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicoById(medicoEntity.IdMedico), Times.Once);

    }
    [Fact(DisplayName = "GetMedicoById deve retornar ERRO quando NÃO Existir no DB")]
    public async Task GetMedicoById_DeveRetornarFalha_QuandoNaoExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var dtoMedico = _fixture.CreateMedicoDTOValid(1, "CARDIOLOGIA", "12345", "SP", true);
        var medicoEntity = _fixture.MedicoValid(dtoMedico);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicoById(medicoEntity.IdMedico))
            .ReturnsAsync((Medico?)null);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetMedicoById(medicoEntity.IdMedico);
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("MedicoNaoEncontrado", result.Error.Id);
        Assert.Equal("O Id do Médico não foi encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicoById(medicoEntity.IdMedico), Times.Once);

    }
    
    [Fact(DisplayName = "GetAllActiveMedicos deve retornar SUCESSO quando existir Médicos Ativos no DB")]
    public async Task GetAllActiveMedicos_DeveRetornarSucesso_QuandoExistirMedicosAtivosNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var listMedico = _fixture.CreateListMedicos(5);
        var listMedicosDto = _fixture.CreateListMedicosDTO(5);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetAllActiveMedicos())
            .ReturnsAsync(listMedico);
        
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<MedicoDTO>>(listMedico))
            .Returns(listMedicosDto);
        
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetAllActiveMedicos();
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetAllActiveMedicos(), Times.Once);

    }
    [Fact(DisplayName = "GetAllActiveMedicos deve retornar ERRO quando NÃO existir Médicos Ativos no DB")]
    public async Task GetAllActiveMedicos_DeveRetornarFalha_QuandoNaoExistirMedicosAtivosNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetAllActiveMedicos())
            .ReturnsAsync((IEnumerable<Medico>?)null);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetAllActiveMedicos();
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("MedicosAtivosNaoEncontrados", result.Error.Id);
        Assert.Equal("Não foram encontrados Médicos ativos na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetAllActiveMedicos(), Times.Once);

    }
    
    [Fact(DisplayName = "GetMedicosByEspecialidadeAndActiveTrue deve retornar SUCESSO quando existir Médicos Ativos para a Especialidade no DB")]
    public async Task GetMedicosByEspecialidadeAndActiveTrue_DeveRetornarSucesso_QuandoExistirMedicosAtivosEParaAEspecialidadeNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var listMedicoByEspecialidade = _fixture.CreateListMedicos(5, "CARDIOLOGIA");
        var listMedicosDto = _fixture.CreateListMedicosDTO(5);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicosByEspecialidadeAndActiveTrue(Especialidades.CARDIOLOGIA))
            .ReturnsAsync(listMedicoByEspecialidade);
        
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<MedicoDTO>>(listMedicoByEspecialidade))
            .Returns(listMedicosDto);
        
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetMedicosByEspecialidadeAndActiveTrue(Especialidades.CARDIOLOGIA);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var firstMedico = result.Value.FirstOrDefault();
        Assert.NotNull(firstMedico);
        Assert.Equal("CARDIOLOGIA", firstMedico.Especialidade.ToString());
        Assert.True(firstMedico.Ativo);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicosByEspecialidadeAndActiveTrue(Especialidades.CARDIOLOGIA), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<MedicoDTO>>(listMedicoByEspecialidade), Times.Once);

    }
    [Fact(DisplayName = "GetMedicosByEspecialidadeAndActiveTrue deve retornar ERRO quando NÃO existir Médicos Ativos para a Especialidade no DB")]
    public async Task GetMedicosByEspecialidadeAndActiveTrue_DeveRetornarFalha_QuandoNaoExistirMedicosAtivosEParaAEspecialidadeNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var listMedicoByEspecialidade = _fixture.CreateListMedicos(5, "CARDIOLOGIA");
        var listMedicosDto = _fixture.CreateListMedicosDTO(5);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicosByEspecialidadeAndActiveTrue(Especialidades.CARDIOLOGIA))
            .ReturnsAsync(listMedicoByEspecialidade);
        
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<MedicoDTO>>(listMedicoByEspecialidade))
            .Returns(listMedicosDto);
        
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetMedicosByEspecialidadeAndActiveTrue(Especialidades.CARDIOLOGIA);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var firstMedico = result.Value.FirstOrDefault();
        Assert.NotNull(firstMedico);
        Assert.Equal("CARDIOLOGIA", firstMedico.Especialidade.ToString());
        Assert.True(firstMedico.Ativo);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicosByEspecialidadeAndActiveTrue(Especialidades.CARDIOLOGIA), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<MedicoDTO>>(listMedicoByEspecialidade), Times.Once);

    }
    
    [Fact(DisplayName = "GetAllMedicos deve retornar SUCESSO quando existirem médicos no DB")]
    public async Task GetAllMedicos_DeveRetornarSucesso_QuandoExistiremMedicosNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var medicos = _fixture.CreateListMedicos(3);
        var medicosDto = _fixture.CreateListMedicosDTO(3);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetAllMedicos())
            .ReturnsAsync(medicos);
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<MedicoDTO>>(medicos))
            .Returns(medicosDto);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetAllMedicos();
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value.Count());
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetAllMedicos(), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<MedicoDTO>>(medicos), Times.Once);
    }
    [Fact(DisplayName = "GetAllMedicos deve retornar ERRO quando não existirem médicos no DB")]
    public async Task GetAllMedicos_DeveRetornarFalha_QuandoNaoExistiremMedicosNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset(); // Adicionando reset do mapper
        // Arrange
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetAllMedicos())
            .ReturnsAsync((IEnumerable<Medico>?)null);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetAllMedicos();
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("MedicosNaoEncontrados", result.Error.Id);
        Assert.Equal("Não foram encontrados Médicos na base de dados.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetAllMedicos(), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<MedicoDTO>>(It.IsAny<IEnumerable<Medico>>()), Times.Never);
    }
    
    [Fact(DisplayName = "GetMedicosByEspecialidade deve retornar SUCESSO quando existirem médicos da especialidade no DB")]
    public async Task GetMedicosByEspecialidade_DeveRetornarSucesso_QuandoExistiremMedicosDaEspecialidadeNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var especialidade = Especialidades.CARDIOLOGIA;
        var medicos = _fixture.CreateListMedicos(2, "CARDIOLOGIA");
        var medicosDto = _fixture.CreateListMedicosDTO(2);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicosByEspecialidade(especialidade))
            .ReturnsAsync(medicos);
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<MedicoDTO>>(medicos))
            .Returns(medicosDto);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetMedicosByEspecialidade(especialidade);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count());
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicosByEspecialidade(especialidade), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<MedicoDTO>>(medicos), Times.Once);
    }
    [Fact(DisplayName = "GetMedicosByEspecialidade deve retornar ERRO quando não existirem médicos da especialidade no DB")]
    public async Task GetMedicosByEspecialidade_DeveRetornarFalha_QuandoNaoExistiremMedicosDaEspecialidadeNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        // Arrange
        var especialidade = Especialidades.DERMATOLOGIA;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicosByEspecialidade(especialidade))
            .ReturnsAsync((IEnumerable<Medico>?)null);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetMedicosByEspecialidade(especialidade);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("NenhumMedicoParaEssaEspecialidade", result.Error.Id);
        Assert.Equal("Nenhum médico encontrado para essa especialidade.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicosByEspecialidade(especialidade), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<MedicoDTO>>(It.IsAny<IEnumerable<Medico>>()), Times.Never);
    }
    
    [Fact(DisplayName = "ExistsMedicoById deve retornar SUCESSO TRUE quando médico existir no DB")]
    public async Task ExistsMedicoById_DeveRetornarSucessoTrue_QuandoMedicoExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var medicoId = 1;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.existsMedicoById(medicoId))
            .ReturnsAsync(true);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.ExistsMedicoById(medicoId);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.existsMedicoById(medicoId), Times.Once);
    }
    [Fact(DisplayName = "ExistsMedicoById deve retornar SUCESSO FALSE quando médico não existir no DB")]
    public async Task ExistsMedicoById_DeveRetornarSucessoFalse_QuandoMedicoNaoExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var medicoId = 999;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.existsMedicoById(medicoId))
            .ReturnsAsync(false);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.ExistsMedicoById(medicoId);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.existsMedicoById(medicoId), Times.Once);
    }
    
    [Fact(DisplayName = "SoftDeleteMedico deve retornar SUCESSO quando médico existir e estiver ativo")]
    public async Task SoftDeleteMedico_DeveRetornarSucesso_QuandoMedicoExistirEEstiverAtivo()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var medicoId = 1;
        var dtoMedico = _fixture.CreateMedicoDTOValid(medicoId, "CARDIOLOGIA", "12345", "SP", true);
        var medicoEntity = _fixture.MedicoValid(dtoMedico);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicoById(medicoId))
            .ReturnsAsync(medicoEntity);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.SoftDeleteMedico(medicoId);
        
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicoById(medicoId), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }
    [Fact(DisplayName = "SoftDeleteMedico deve retornar ERRO quando médico não existir no DB")]
    public async Task SoftDeleteMedico_DeveRetornarFalha_QuandoMedicoNaoExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var medicoId = 999;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicoById(medicoId))
            .ReturnsAsync((Medico?)null);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.SoftDeleteMedico(medicoId);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("MedicoNaoEncontrado", result.Error.Id);
        Assert.Equal("O Id do Médico não foi encontrado na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicoById(medicoId), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }
    [Fact(DisplayName = "SoftDeleteMedico deve retornar ERRO quando médico já estiver inativo")]
    public async Task SoftDeleteMedico_DeveRetornarFalha_QuandoMedicoJaEstiverInativo()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var medicoId = 1;
        var dtoMedico = _fixture.CreateMedicoDTOValid(medicoId, "CARDIOLOGIA", "12345", "SP", false);
        var medicoEntity = _fixture.MedicoValid(dtoMedico);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicoById(medicoId))
            .ReturnsAsync(medicoEntity);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.SoftDeleteMedico(medicoId);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("MedicoJaInativo", result.Error.Id);
        Assert.Equal("O Médico já consta inativo e não pode ser desativado.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicoById(medicoId), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }
    
    [Fact(DisplayName = "GetMedicoByCRM deve retornar SUCESSO quando CRM existir no DB")]
    public async Task GetMedicoByCRM_DeveRetornarSucesso_QuandoCRMExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var crmNumber = "12345";
        var dtoMedico = _fixture.CreateMedicoDTOValid(1, "CARDIOLOGIA", crmNumber, "SP", true);
        var medicoEntity = _fixture.MedicoValid(dtoMedico);
        var medicoDto = _fixture.CreateMedicoDTO(medicoEntity);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicoByCRM(crmNumber))
            .ReturnsAsync(medicoEntity);
        _fixture.MockMapper.Setup(m => m.Map<MedicoDTO>(medicoEntity))
            .Returns(medicoDto);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetMedicoByCRM(crmNumber);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(crmNumber, result.Value.CrmNumber);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicoByCRM(crmNumber), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<MedicoDTO>(medicoEntity), Times.Once);
    }
    [Fact(DisplayName = "GetMedicoByCRM deve retornar ERRO quando CRM não existir no DB")]
    public async Task GetMedicoByCRM_DeveRetornarFalha_QuandoCRMNaoExistirNoDB()
    {
        _fixture.MockUnitOfWork.Reset();
        // Arrange
        var crmNumber = "99999";
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Medicos.GetMedicoByCRM(crmNumber))
            .ReturnsAsync((Medico?)null);
        
        // Act
        var service = _fixture.CreateMedicoService();
        var result = await service.GetMedicoByCRM(crmNumber);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("CrmNaoLocalizado", result.Error.Id);
        Assert.Equal("Não foi encontrado um Médico com este CRM na base.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        _fixture.MockUnitOfWork.Verify(uow => uow.Medicos.GetMedicoByCRM(crmNumber), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<MedicoDTO>(It.IsAny<Medico>()), Times.Never);
    }
}
