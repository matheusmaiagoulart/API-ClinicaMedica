using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Domain.ValueObjects;

public class Endereco
{
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public Estados Estado { get; private set; }
    public string Cep { get; private set; }
   
    protected Endereco(){}
    
    public Endereco(string logradouro, string numero, string complemento, string bairro, string cidade, Estados estado, string cep)
    {
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
    }
    
    
}