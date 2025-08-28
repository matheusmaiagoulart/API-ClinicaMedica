using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API_ClinicaMedica.Domain.ValueObjects;
using DevOne.Security.Cryptography.BCrypt;

namespace API_ClinicaMedica.Domain.Entities;
[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int IdUsuario { get; private set; }
    public string Email { get; private set; }
    public string Senha { get; private set; }
    
    public InformacoesBasicas InformacoesBasicas { get; private set; }
    public Endereco Endereco { get; private set; }

    protected Usuario()
    {
        
    }

    public Usuario( string email, string senha, InformacoesBasicas infos, Endereco endereco)
    {
        InformacoesBasicas = infos;
        this.Email = email;
        this.Senha = senha;
        this.Endereco = endereco;
    }

    public void AtualizaInformacaoBasica(InformacoesBasicas informacoesBasicas)
    {
        this.InformacoesBasicas = informacoesBasicas;
    }
    
    public void AtualizaEndereco(Endereco endereco)
    {
        this.Endereco = endereco;
    }

    public void HashSenha(string senha)
    {
        var senhaHashed = BCryptHelper.HashPassword(senha, BCryptHelper.GenerateSalt());
        this.Senha = senhaHashed;
    }
}