using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;
using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;

namespace API_ClinicaMedica.Application.DTOs.UpdateUsuarioDTOs;

public class UpdateUsuarioDTO
{
    public int IdUsuario { get; set; }
    public string? Email { get; set; }
     
    public string? Senha { get; set; }

    public InformacoesBasicasUpdateDTO? InformacoesBasicas { get; set; } = new InformacoesBasicasUpdateDTO();

    public EnderecoUpdateDTO? Endereco { get; set; } = new EnderecoUpdateDTO();
}
