using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.DTOs.ValueObjectsDTOs;
using API_ClinicaMedica.Domain.ValueObjects;

namespace API_ClinicaMedica.Domain.Entities;

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

}