namespace API_ClinicaMedica.Domain.ValueObjects;

public class InformacoesBasicas
{
    private string Nome { get; set; }
    private string Email { get; set; }
    private string Telefone { get; set; }
    private DateTime DataNascimento { get; set; }
    private string Cpf { get; set; }
    private string Rg { get; set; }
    
    
    
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