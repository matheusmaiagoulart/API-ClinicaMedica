using System.ComponentModel.DataAnnotations;

namespace API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;

public class InformacoesBasicasDTO
{
    
    public string Nome { get; init; }
    
    public string Telefone { get; init; }
    
    public DateTime DataNascimento { get; init; }
    
    public string Cpf { get; init; }
    
    public string Rg { get; init; }
}