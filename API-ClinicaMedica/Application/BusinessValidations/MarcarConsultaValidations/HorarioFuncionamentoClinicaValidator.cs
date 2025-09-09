using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Infra.Interfaces;

namespace API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;

public class HorarioFuncionamentoClinicaValidator : IMarcarConsultaValidator
{
    public async Task<Result> Validacao(CreateConsultaDTO dto)
    {
        var dataConsulta = dto.DataHoraConsulta;
        
        if (dataConsulta.DayOfWeek == DayOfWeek.Saturday || dataConsulta.DayOfWeek == DayOfWeek.Sunday)
        {
            return Result.Failure(ConsultaErrosResult.DataForaDosDiasUteis());
        }
        
        var aberturaClinica = dataConsulta.Date.AddHours(8);  // 08:00 do dia da consulta
        var fechamentoClinica = dataConsulta.Date.AddHours(18); // 18:00 do dia da consulta
        
        if (dataConsulta < aberturaClinica || dataConsulta >= fechamentoClinica)
        {
            return Result.Failure(ConsultaErrosResult.DataForaDoHorarioDeAtendimento());
        }

        return Result.Success();
    }
}