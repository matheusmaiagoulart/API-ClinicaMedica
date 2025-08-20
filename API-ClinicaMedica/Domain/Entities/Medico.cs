using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Domain.ValueObjects;

namespace API_ClinicaMedica.Domain.Entities;

public class Medico
{
    [Key]
    public int IdMedico { get; }
    public Usuario Usuario { get; set; }
    public Especialidades Especialidade { get; private set;}
    
    public string Crm { get; private set;}
    public bool Ativo { get; private set; }

    protected Medico()
    {
        
    }
    public Medico(Usuario usuario, Especialidades especialidade, string crm, bool ativo)
    {
        Usuario = usuario;
        Especialidade = especialidade;
        Crm = crm;
        Ativo = ativo;
    }
}