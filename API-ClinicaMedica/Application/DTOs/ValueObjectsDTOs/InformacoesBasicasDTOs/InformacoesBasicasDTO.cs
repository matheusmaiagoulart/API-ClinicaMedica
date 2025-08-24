using System.ComponentModel.DataAnnotations;

namespace API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;

public class InformacoesBasicasDTO
{
    [Required, MaxLength(100)]
    public string Nome { get; init; }
    
    [Required, MaxLength(11)]
    [RegularExpression("^\\d{11}$", ErrorMessage = "O campo Telefone deve conter apenas números e ter 11 dígitos.")]
    public string Telefone { get; init; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; init; }
    
    [Required]
    [RegularExpression("^\\d{11}$", ErrorMessage = "O campo CPF deve conter apenas números e ter 11 dígitos.")]
    public string Cpf { get; init; }
    
    [Required, MaxLength(9)]
    [RegularExpression("^\\d{9}$", ErrorMessage = "O campo RG deve conter apenas números e ter até 9 dígitos.")]
    public string Rg { get; init; }
}