using API_ClinicaMedica.Application.Constants;
using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Infra.Interfaces;
using AutoMapper.Configuration.Annotations;

namespace API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;


public class ValidarHoraConsulta : IMarcarConsultaValidator
{
    
    public async Task<Result> Validacao(CreateConsultaDTO dto)
    {
        //Garante que a consulta seja marcada dentro dos horários permitidos
        
        var horaConsulta = dto.DataHoraConsulta.TimeOfDay;

        if (!HorariosConsultas.Horarios().Contains(horaConsulta))
        {
            var proximaConsulta = HorariosConsultas.Horarios()
                .FirstOrDefault(horario => horario > horaConsulta);
            
            if(proximaConsulta == TimeSpan.Zero)
            {
                var proxima = (TimeSpan) HorariosConsultas.Horarios().GetValue(0);
                return Result.Failure(ConsultaErrosResult.HorarioNaoPermitido(proxima)); 
            }
            
            return Result.Failure(ConsultaErrosResult.HorarioNaoPermitido(proximaConsulta));
        }
        return Result.Success();
    }
}