using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;

namespace API_ClinicaMedica.Application.DTOs.UsuarioDTOs;

public class UniqueFieldsValidationDTO
{
    public int IdUsuario { get; set; }
    public string Email { get; set; }

    public InformacoesBasicasFieldValidationDTO InformacoesBasicas { get; set; }

}