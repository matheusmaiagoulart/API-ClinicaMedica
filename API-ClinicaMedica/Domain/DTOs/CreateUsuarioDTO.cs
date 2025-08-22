using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.DTOs.ValueObjectsDTOs;


namespace API_ClinicaMedica.Domain.DTOs;

public class CreateUsuarioDTO
{
    [Required, EmailAddress] 
    public string Email { get; set; }
    [Required] 
    public string Senha { get; set; }
    public InformacoesBasicasDTO InformacoesBasicas { get; set; }
    public EnderecoDTO Endereco { get; set; }
}