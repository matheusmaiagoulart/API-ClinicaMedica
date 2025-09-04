using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results.GenericsResults;

namespace API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;

public interface IMarcarConsultaValidator
{
    Task<Result> Validacao(CreateConsultaDTO dto);
}