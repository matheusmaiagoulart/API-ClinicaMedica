using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;


namespace API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;

public interface IValidacaoInformacoesBasicas
{
    Task validacao(UniqueFieldsValidationDTO dto);
}