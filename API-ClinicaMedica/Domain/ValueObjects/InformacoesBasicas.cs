using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API_ClinicaMedica.Domain.ValueObjects;

public class InformacoesBasicas
{
    public string Nome { get; private set; }
    public string Telefone { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string Cpf { get; private set; }
    public string Rg { get; private set; }
    
    protected InformacoesBasicas() { }
    
    public InformacoesBasicas(string nome, string telefone, DateTime dataNascimento, string cpf, string rg)
    {
        Nome = nome;
        Telefone = telefone;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        Rg = rg;
    }
}