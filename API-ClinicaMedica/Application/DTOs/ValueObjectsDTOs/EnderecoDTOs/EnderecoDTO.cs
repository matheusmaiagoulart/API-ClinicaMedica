using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;

public class EnderecoDTO
{
   
    public string Logradouro { get; set; }
    
    public string Numero { get; set; }
    
    public string Complemento { get; set; }
    
    public string Bairro { get; set; }
    
    public string Cidade { get; set; }
    
    public Estados Estado { get; set; }
    
    public string Cep { get; set; }
}