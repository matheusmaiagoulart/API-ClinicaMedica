using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Tests.Application.Fixtures;
using Moq;

namespace API_ClinicaMedica.Tests.Application.Services;

public class ConsultaService_Test : IClassFixture<ConsultaServiceFixture>
{
    private readonly ConsultaServiceFixture _fixture;
    
    public ConsultaService_Test(ConsultaServiceFixture fixture)
    {
        _fixture = fixture;
    }

    #region AgendarConsulta Tests

    [Fact(DisplayName = "AgendarConsulta deve retornar SUCESSO quando dados forem VÁLIDOS")]
    public async Task AgendarConsulta_DeveRetornarSucesso_QuandoDadosForemValidos()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var dto = _fixture.CreateConsultaDTOValid();
        var consultaEntity = _fixture.ConsultaValid(dto);
        
        _fixture.MockMarcarConsultaValidator.Setup(v => v.Validacao(dto))
            .Returns(Task.FromResult(Result.Success()));
        _fixture.MockMapper.Setup(m => m.Map<Consulta>(dto))
            .Returns(consultaEntity);
        
        _fixture.MockUnitOfWork.Setup(u => u.Consultas.AddAsync(consultaEntity))
            .Returns(Task.CompletedTask);
        _fixture.MockUnitOfWork.Setup(u => u.CommitAsync())
            .ReturnsAsync(true);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.AgendarConsulta(dto);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(dto.IdPaciente, result.Value.IdPaciente);
        Assert.Equal(dto.IdMedico, result.Value.IdMedico);
        Assert.Equal(dto.DataHoraConsulta, result.Value.DataHoraConsulta);
        Assert.Equal(dto.Especialidade, result.Value.Especialidade);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.AddAsync(It.IsAny<Consulta>()), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact(DisplayName = "AgendarConsulta deve retornar ERRO quando validação de negócio falhar")]
    public async Task AgendarConsulta_DeveRetornarFalha_QuandoValidacaoDeNegocioFalhar()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var dto = _fixture.CreateConsultaDTOValid();
        var error = new Error("MedicoNaoDisponivelNaDataEscolhida", "O médico não está disponível na data escolhida.", 400);
        
        _fixture.MockMarcarConsultaValidator.Setup(v => v.Validacao(dto))
            .Returns(Task.FromResult(Result.Failure(error)));
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.AgendarConsulta(dto);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("MedicoNaoDisponivelNaDataEscolhida", result.Error.Id);
        Assert.Equal("O médico não está disponível na data escolhida.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.AddAsync(It.IsAny<Consulta>()), Times.Never);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "AgendarConsulta deve retornar ERRO quando horário for inválido")]
    public async Task AgendarConsulta_DeveRetornarFalha_QuandoHorarioForInvalido()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange - usando horário inválido (19:00 - fora do horário de funcionamento)
        var dataInvalida = DateTime.Now.AddDays(1).Date.AddHours(19);
        var dto = _fixture.CreateConsultaDTOValid(dataHoraConsulta: dataInvalida);
        
        _fixture.MockMarcarConsultaValidator.Setup(v => v.Validacao(dto))
            .Returns(Task.FromResult(Result.Success()));
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.AgendarConsulta(dto);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("HorarioNaoPermitido", result.Error.Id);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.AddAsync(It.IsAny<Consulta>()), Times.Never);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    #endregion

    #region CancelarConsulta Tests

    [Fact(DisplayName = "CancelarConsulta deve retornar SUCESSO quando consulta existir e estiver dentro do prazo")]
    public async Task CancelarConsulta_DeveRetornarSucesso_QuandoConsultaExistirEEstiverDentroDoPrazo()
    {
        _fixture.MockUnitOfWork.Reset();
        
        // Arrange
        var consultaId = Guid.NewGuid();
        var dataFutura = DateTime.Now.AddDays(2).Date.AddHours(10); // 2 dias no futuro, às 10h
        var consulta = _fixture.CreateConsultaWithSpecificDate(dataFutura, true);
        var motivo = MotivosCancelamentoConsulta.MOTIVO_DE_SAUDE;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultaById(consultaId))
            .ReturnsAsync(consulta);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.CancelarConsulta(consultaId, motivo);
        
        // Assert
        Assert.True(result.IsSuccess);
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.UpdateAsync(It.IsAny<Consulta>()), Times.Once);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact(DisplayName = "CancelarConsulta deve retornar ERRO quando consulta não existir")]
    public async Task CancelarConsulta_DeveRetornarFalha_QuandoConsultaNaoExistir()
    {
        _fixture.MockUnitOfWork.Reset();
        
        // Arrange
        var consultaId = Guid.NewGuid();
        var motivo = MotivosCancelamentoConsulta.MOTIVOS_PESSOAIS;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultaById(consultaId))
            .ReturnsAsync((Consulta?)null);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.CancelarConsulta(consultaId, motivo);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("ConsultaNaoEncontrada", result.Error.Id);
        Assert.Equal("Consulta não encontrada na base de dados.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.UpdateAsync(It.IsAny<Consulta>()), Times.Never);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "CancelarConsulta deve retornar ERRO quando consulta já estiver inativa")]
    public async Task CancelarConsulta_DeveRetornarFalha_QuandoConsultaJaEstiverInativa()
    {
        _fixture.MockUnitOfWork.Reset();
        
        // Arrange
        var consultaId = Guid.NewGuid();
        var dataFutura = DateTime.Now.AddDays(2).Date.AddHours(10);
        var consulta = _fixture.CreateConsultaWithSpecificDate(dataFutura, false); // Consulta inativa
        var motivo = MotivosCancelamentoConsulta.MOTIVOS_PESSOAIS;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultaById(consultaId))
            .ReturnsAsync(consulta);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.CancelarConsulta(consultaId, motivo);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("ConsultaNaoEncontrada", result.Error.Id);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.UpdateAsync(It.IsAny<Consulta>()), Times.Never);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    [Fact(DisplayName = "CancelarConsulta deve retornar ERRO quando cancelamento for fora do prazo")]
    public async Task CancelarConsulta_DeveRetornarFalha_QuandoCancelamentoForForaDoPrazo()
    {
        _fixture.MockUnitOfWork.Reset();
        
        // Arrange
        var consultaId = Guid.NewGuid();
        var dataProxima = DateTime.Now.AddHours(12); // Menos de 24h
        var consulta = _fixture.CreateConsultaWithSpecificDate(dataProxima, true);
        var motivo = MotivosCancelamentoConsulta.MOTIVOS_PESSOAIS;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultaById(consultaId))
            .ReturnsAsync(consulta);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.CancelarConsulta(consultaId, motivo);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("CancelamentoForaDoPrazo", result.Error.Id);
        Assert.Equal("O cancelamento só pode ser feito com no mínimo 24h de antecedência.", result.Error.mensagem);
        Assert.Equal(400, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.UpdateAsync(It.IsAny<Consulta>()), Times.Never);
        _fixture.MockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
    }

    #endregion

    #region GetConsultasByPacienteId Tests

    [Fact(DisplayName = "GetConsultasByPacienteId deve retornar SUCESSO quando paciente tiver consultas")]
    public async Task GetConsultasByPacienteId_DeveRetornarSucesso_QuandoPacienteTiverConsultas()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var pacienteId = 1;
        var consultas = _fixture.CreateListConsultas(3);
        var consultasDto = _fixture.CreateListConsultasViewDTO(3);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultasByPacienteId(pacienteId))
            .ReturnsAsync(consultas);
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<ConsultaViewDTO>>(consultas))
            .Returns(consultasDto);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.GetConsultasByPacienteId(pacienteId);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value.Count());
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.GetConsultasByPacienteId(pacienteId), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<ConsultaViewDTO>>(consultas), Times.Once);
    }

    [Fact(DisplayName = "GetConsultasByPacienteId deve retornar ERRO quando paciente não tiver consultas")]
    public async Task GetConsultasByPacienteId_DeveRetornarFalha_QuandoPacienteNaoTiverConsultas()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var pacienteId = 999;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultasByPacienteId(pacienteId))
            .ReturnsAsync((IEnumerable<Consulta>)null!);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.GetConsultasByPacienteId(pacienteId);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("PacienteSemConsultas", result.Error.Id);
        Assert.Equal("Esse paciente não possui consultas.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.GetConsultasByPacienteId(pacienteId), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<ConsultaViewDTO>>(It.IsAny<IEnumerable<Consulta>>()), Times.Never);
    }

    #endregion

    #region GetConsultasByMedicoId Tests

    [Fact(DisplayName = "GetConsultasByMedicoId deve retornar SUCESSO quando médico tiver consultas")]
    public async Task GetConsultasByMedicoId_DeveRetornarSucesso_QuandoMedicoTiverConsultas()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var medicoId = 1;
        var consultas = _fixture.CreateListConsultas(2);
        var consultasDto = _fixture.CreateListConsultasViewDTO(2);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultasByMedicoId(medicoId))
            .ReturnsAsync(consultas);
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<ConsultaViewDTO>>(consultas))
            .Returns(consultasDto);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.GetConsultasByMedicoId(medicoId);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count());
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.GetConsultasByMedicoId(medicoId), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<ConsultaViewDTO>>(consultas), Times.Once);
    }

    [Fact(DisplayName = "GetConsultasByMedicoId deve retornar ERRO quando médico não tiver consultas")]
    public async Task GetConsultasByMedicoId_DeveRetornarFalha_QuandoMedicoNaoTiverConsultas()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var medicoId = 999;
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultasByMedicoId(medicoId))
            .ReturnsAsync((IEnumerable<Consulta>?)null);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.GetConsultasByMedicoId(medicoId);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("MedicoSemConsultas", result.Error.Id);
        Assert.Equal("Esse Médico não possui consultas.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.GetConsultasByMedicoId(medicoId), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<ConsultaViewDTO>>(It.IsAny<IEnumerable<Consulta>>()), Times.Never);
    }

    #endregion

    #region GetConsultasByDateRange Tests

    [Fact(DisplayName = "GetConsultasByDateRange deve retornar SUCESSO quando existirem consultas no intervalo")]
    public async Task GetConsultasByDateRange_DeveRetornarSucesso_QuandoExistiremConsultasNoIntervalo()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var startDate = DateTime.Now.Date;
        var endDate = DateTime.Now.Date.AddDays(7);
        var consultas = _fixture.CreateListConsultas(4);
        var consultasDto = _fixture.CreateListConsultasViewDTO(4);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultasByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(consultas);
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<ConsultaViewDTO>>(consultas))
            .Returns(consultasDto);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.GetConsultasByDateRange(startDate, endDate);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(4, result.Value.Count());
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.GetConsultasByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<ConsultaViewDTO>>(consultas), Times.Once);
    }

    [Fact(DisplayName = "GetConsultasByDateRange deve retornar ERRO quando não existirem consultas no intervalo")]
    public async Task GetConsultasByDateRange_DeveRetornarFalha_QuandoNaoExistiremConsultasNoIntervalo()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var startDate = DateTime.Now.Date.AddMonths(6);
        var endDate = DateTime.Now.Date.AddMonths(6).AddDays(7);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetConsultasByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync((IEnumerable<Consulta>?)null);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.GetConsultasByDateRange(startDate, endDate);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("SemConsultasParaEsteIntervalo", result.Error.Id);
        Assert.Equal("Não há consultas para o intervalo procurado.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.GetConsultasByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<ConsultaViewDTO>>(It.IsAny<IEnumerable<Consulta>>()), Times.Never);
    }

    #endregion

    #region GetMedicoConsultasByDateRange Tests

    [Fact(DisplayName = "GetMedicoConsultasByDateRange deve retornar SUCESSO quando existirem consultas do médico no intervalo")]
    public async Task GetMedicoConsultasByDateRange_DeveRetornarSucesso_QuandoExistiremConsultasDoMedicoNoIntervalo()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var medicoId = 1;
        var startDate = DateTime.Now.Date;
        var endDate = DateTime.Now.Date.AddDays(7);
        var consultas = _fixture.CreateListConsultas(2);
        var consultasDto = _fixture.CreateListConsultasViewDTO(2);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetMedicoConsultasByDateRange(medicoId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(consultas);
        _fixture.MockMapper.Setup(m => m.Map<IEnumerable<ConsultaViewDTO>>(consultas))
            .Returns(consultasDto);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.GetMedicoConsultasByDateRange(medicoId, startDate, endDate);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count());
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.GetMedicoConsultasByDateRange(medicoId, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<ConsultaViewDTO>>(consultas), Times.Once);
    }

    [Fact(DisplayName = "GetMedicoConsultasByDateRange deve retornar ERRO quando não existirem consultas do médico no intervalo")]
    public async Task GetMedicoConsultasByDateRange_DeveRetornarFalha_QuandoNaoExistiremConsultasDoMedicoNoIntervalo()
    {
        _fixture.MockUnitOfWork.Reset();
        _fixture.MockMapper.Reset();
        
        // Arrange
        var medicoId = 999;
        var startDate = DateTime.Now.Date.AddMonths(6);
        var endDate = DateTime.Now.Date.AddMonths(6).AddDays(7);
        
        _fixture.MockUnitOfWork.Setup(uow => uow.Consultas.GetMedicoConsultasByDateRange(medicoId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync((IEnumerable<Consulta>?)null);
        
        // Act
        var service = _fixture.CreateConsultaService();
        var result = await service.GetMedicoConsultasByDateRange(medicoId, startDate, endDate);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal("SemConsultasParaEsteIntervalo", result.Error.Id);
        Assert.Equal("Não há consultas para o intervalo procurado.", result.Error.mensagem);
        Assert.Equal(404, result.Error.StatusCode);
        
        _fixture.MockUnitOfWork.Verify(uow => uow.Consultas.GetMedicoConsultasByDateRange(medicoId, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        _fixture.MockMapper.Verify(m => m.Map<IEnumerable<ConsultaViewDTO>>(It.IsAny<IEnumerable<Consulta>>()), Times.Never);
    }

    #endregion

    #region Debug Tests

    [Fact(DisplayName = "DEBUG - Verificar horários válidos")]
    public void Debug_VerificarHorariosValidos()
    {
        // Arrange
        var horarios = API_ClinicaMedica.Application.Constants.HorariosConsultas.Horarios();
        var horasTeste = new TimeSpan(10, 0, 0);
        
        // Debug
        var horariosString = string.Join(", ", horarios.Select(h => h.ToString(@"hh\:mm")));
        Console.WriteLine($"Horários válidos: {horariosString}");
        Console.WriteLine($"Hora do teste: {horasTeste:hh\\:mm}");
        Console.WriteLine($"Contém hora do teste: {horarios.Contains(horasTeste)}");
        
        // Assert
        Assert.Contains(horasTeste, horarios);
    }

    [Fact(DisplayName = "DEBUG - Teste simples de criação de consulta")]
    public void Debug_TesteSimplesCriacaoConsulta()
    {
        // Arrange
        var dto = _fixture.CreateConsultaDTOValid();
        
        // Debug - verificar se o DTO está sendo criado corretamente
        Assert.NotNull(dto);
        Assert.True(dto.IdPaciente > 0);
        Assert.True(dto.IdMedico > 0);
        Assert.NotEqual(DateTime.MinValue, dto.DataHoraConsulta);
        
        // Tentar criar a entidade diretamente
        var consulta = _fixture.ConsultaValid(dto);
        
        Assert.NotNull(consulta);
        Assert.Equal(dto.IdPaciente, consulta.IdPaciente);
        Assert.Equal(dto.IdMedico, consulta.IdMedico);
    }

    [Fact(DisplayName = "DEBUG - Teste mapper mock")]
    public void Debug_TesteMapperMock()
    {
        _fixture.MockMapper.Reset();
        
        // Arrange
        var dto = _fixture.CreateConsultaDTOValid();
        var consultaEntity = _fixture.ConsultaValid(dto);
        
        _fixture.MockMapper.Setup(m => m.Map<Consulta>(dto))
            .Returns(consultaEntity);
        
        // Act
        var result = _fixture.MockMapper.Object.Map<Consulta>(dto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.IdPaciente, result.IdPaciente);
    }

    #endregion
}
