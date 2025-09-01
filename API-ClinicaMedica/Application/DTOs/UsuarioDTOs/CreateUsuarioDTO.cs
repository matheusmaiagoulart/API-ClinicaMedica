
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;

namespace API_ClinicaMedica.Application.DTOs.UsuarioDTOs;

public class CreateUsuarioDTO
{
    
    public string Email { get; set; }
    
    public string Senha { get; set; }
    public InformacoesBasicasDTO InformacoesBasicas { get; set; }
    public EnderecoDTO Endereco { get; set; }
}