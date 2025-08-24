using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;

public class EnderecoUpdateDTO
{
  
    [MaxLength(100)]
    public string? Logradouro { get; set; }
    
    [MaxLength(10)]
    public string? Numero { get; set; }
    
    [MaxLength(50)]
    public string? Complemento { get; set; }
    
    [MaxLength(50)]
    public string? Bairro { get; set; }
    
    [MaxLength(50)]
    public string? Cidade { get; set; }
    
    [EnumDataType(typeof(Estados))]
    public Estados? Estado { get; set; }
    
    [MaxLength(8)]
    public string? Cep { get; set; }
}