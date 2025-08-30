using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;


namespace API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;

public interface IValidacaoInformacoesBasicas
{
    Task<Result> validacao(UniqueFieldsValidationDTO dto);
}