using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;

public class EnderecoDTO
{
    [Required, MaxLength(100)]
    public string Logradouro { get; set; }
    
    [Required, MaxLength(10)]
    public string Numero { get; set; }
    
    [MaxLength(50)]
    public string Complemento { get; set; }
    
    [Required, MaxLength(50)]
    public string Bairro { get; set; }
    
    [Required, MaxLength(50)]
    public string Cidade { get; set; }
    
    [Required]
    [EnumDataType(typeof(Estados))]
    public Estados Estado { get; set; }
    
    [Required, MaxLength(8)]
    public string Cep { get; set; }
}