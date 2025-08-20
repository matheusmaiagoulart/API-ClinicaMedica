namespace API_ClinicaMedica.Domain.ValueObjects;

public class InformacoesBasicas
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Telefone { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string Cpf { get; private set; }
    public string Rg { get; private set; }
    
    
    
    protected InformacoesBasicas() { }
    
    public InformacoesBasicas(string nome, string email, string telefone, DateTime dataNascimento, string cpf, string rg)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        Rg = rg;
        
    }
}