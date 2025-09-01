using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results.GenericsResults;

namespace API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Interface;

public interface IValidacaoInformacoesBasicas
{
    Task<Result> Validacao(UniqueFieldsValidationDTO dto);
}