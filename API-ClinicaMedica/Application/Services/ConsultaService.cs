using API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;
using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Interfaces;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Infra.Interfaces;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace API_ClinicaMedica.Application.Services;

public class ConsultaService : IConsultaService
{
    private readonly IMapper _mapper;
    private List<IMarcarConsultaValidator> ValidacoesConsulta;
    private readonly ValidarHoraConsulta _horaConsulta;
    private readonly IUnitOfWork _unitOfWork;

    public ConsultaService(IMapper mapper, IEnumerable<IMarcarConsultaValidator> consultaValidator, IUnitOfWork unitOfWork, ValidarHoraConsulta horaConsulta)
    {
        _mapper = mapper;
        ValidacoesConsulta = consultaValidator.ToList();
        _unitOfWork = unitOfWork;
        _horaConsulta = horaConsulta;
    }

    public async Task<Result<Consulta>> AgendarConsulta(CreateConsultaDTO dto)
    {
        // Validação da hora da consulta antes das outras validações
        var validarHorarioConsulta = await _horaConsulta.Validacao(dto);
        if (validarHorarioConsulta.IsFailure)
            return Result<Consulta>.Failure(validarHorarioConsulta.Error);
        
        foreach (var index in ValidacoesConsulta)
        {
            // Validações seguintes - CORRIGIDO: await no método Validacao
            var result = await index.Validacao(dto);
            if (result.IsFailure)
            {
                return Result<Consulta>.Failure(result.Error);
            }
        }

        var consultaAgendada = _mapper.Map<Consulta>(dto);
        await _unitOfWork.Consultas.AddAsync(consultaAgendada);
        await _unitOfWork.CommitAsync();
        
        return Result<Consulta>.Success(consultaAgendada);
    }
    

    public async Task<Result> CancelarConsulta(Guid id, MotivosCancelamentoConsulta motivo)
    {
        var consulta = _unitOfWork.Consultas.GetConsultaById(id).Result;
        if (consulta == null || !consulta.Ativo)
            return Result.Failure(ConsultaErrosResult.ConsultaNaoEncontrada());
        
        if(consulta.DataHoraConsulta < DateTime.Now.AddHours(24))
            return Result.Failure(ConsultaErrosResult.CancelamentoForaDoPrazo()); //24h de antecedência
        
        consulta.CancelarConsulta(motivo);
        await _unitOfWork.Consultas.UpdateAsync(consulta);
        await _unitOfWork.CommitAsync();
        return Result.Success();
        
    }

    
    public async Task<Result<IEnumerable<ConsultaViewDTO>>> GetConsultasByPacienteId(int pacienteId)
    {
        var consultas = await _unitOfWork.Consultas.GetConsultasByPacienteId(pacienteId);
        if(consultas.IsNullOrEmpty())
            return Result<IEnumerable<ConsultaViewDTO>>.Failure(ConsultaErrosResult.PacienteSemConsultas());
        
        var consultasMapped = _mapper.Map<IEnumerable<ConsultaViewDTO>>(consultas);
        return Result<IEnumerable<ConsultaViewDTO>>.Success(consultasMapped);
    }
    

    public async Task<Result<IEnumerable<ConsultaViewDTO>>> GetConsultasByMedicoId(int medicoId)
    {
        var consultas = await _unitOfWork.Consultas.GetConsultasByMedicoId(medicoId);
        if(consultas.IsNullOrEmpty())
            return Result<IEnumerable<ConsultaViewDTO>>.Failure(ConsultaErrosResult.MedicoSemConsultas());
        
        var consultasMapped = _mapper.Map<IEnumerable<ConsultaViewDTO>>(consultas);
        return Result<IEnumerable<ConsultaViewDTO>>.Success(consultasMapped);
    }

    
    public async Task<Result<IEnumerable<ConsultaViewDTO>>> GetConsultasByDateRange(DateTime startDate, DateTime endDate)
    {
        ZerarDateTime(startDate, endDate);
        
        var consultas = await _unitOfWork.Consultas.GetConsultasByDateRange(startDate, endDate);
        if(consultas.IsNullOrEmpty())
            return Result<IEnumerable<ConsultaViewDTO>>.Failure(ConsultaErrosResult.SemConsultasParaEsteIntervalo());
        
        var consultasMapped = _mapper.Map<IEnumerable<ConsultaViewDTO>>(consultas);
        return Result<IEnumerable<ConsultaViewDTO>>.Success(consultasMapped);
    }

    public async Task<Result<IEnumerable<ConsultaViewDTO>>> GetMedicoConsultasByDateRange(int idMedico, DateTime startDate, DateTime endDate)
    {
        ZerarDateTime(startDate, endDate);
        
        var consultas = await _unitOfWork.Consultas.GetMedicoConsultasByDateRange(idMedico, startDate, endDate);
        if(consultas.IsNullOrEmpty())
            return Result<IEnumerable<ConsultaViewDTO>>.Failure(ConsultaErrosResult.SemConsultasParaEsteIntervalo());
        
        var consultasMapped = _mapper.Map<IEnumerable<ConsultaViewDTO>>(consultas);
        return Result<IEnumerable<ConsultaViewDTO>>.Success(consultasMapped);
    }
    
    public void ZerarDateTime(DateTime startDate, DateTime endDate)
    {
        startDate.Date.AddSeconds(0);
        startDate.Date.AddMicroseconds(0);
        startDate.Date.AddMilliseconds(0);
        
        endDate.Date.AddSeconds(0);
        endDate.Date.AddMicroseconds(0);
        endDate.Date.AddMilliseconds(0);
    }
}