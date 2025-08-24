using API_ClinicaMedica.Domain.ValueObjects;

namespace API_ClinicaMedica.Application.DTOs.UsuarioDTOs;

public class UsuarioDTO
{
    public int IdUsuario { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    
    public InformacoesBasicas InformacoesBasicas { get; set; }
    public Endereco Endereco { get; set; }
}