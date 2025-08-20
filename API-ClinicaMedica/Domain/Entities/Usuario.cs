using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.ValueObjects;

namespace API_ClinicaMedica.Domain.Entities;

public class Usuario
{
    [Key]
    public int IdUsuario { get; }
    public string Login { get; }
    public string Senha { get; }
    
    public InformacoesBasicas InformacoesBasicas { get; private set; }
    public Endereco Endereco { get; private set; }

    protected Usuario()
    {
        
    }

    public Usuario( string login, string senha, InformacoesBasicas infos)
    {
        InformacoesBasicas = infos;
        this.Login = login;
        this.Senha = senha;
    }

}