using API_ClinicaMedica.Domain.DTOs;

namespace API_ClinicaMedica.Application.Validations.Interface.UsuarioValidationInformacoesBasicas;

public interface IValidacaoInformacoesBasicas
{
    void validacao(CreateUsuarioDTO dto);
}