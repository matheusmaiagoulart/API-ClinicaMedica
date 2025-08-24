using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;

namespace API_ClinicaMedica.Application.DTOs.UsuarioDTOs;

public class CreateUsuarioDTO
{
    [Required, EmailAddress] 
    public string Email { get; set; }
    [Required] 
    public string Senha { get; set; }
    public InformacoesBasicasDTO InformacoesBasicas { get; set; }
    public EnderecoDTO Endereco { get; set; }
}