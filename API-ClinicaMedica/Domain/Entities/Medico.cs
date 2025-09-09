using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Domain.Entities;

[Table("Medicos")]
public class Medico
{
    [Key]
    public int IdMedico { get; private set; }
    public Usuario Usuario { get; private set; }
    public Especialidades Especialidade { get; private set;}
    public Estados UfCrm { get; private set; }
    public string CrmNumber { get; private set;}
    public bool Ativo { get; private set; }

    protected Medico()
    {
        
    }
    public Medico(int IdUsuario, Especialidades especialidade, string crmNumber, bool? ativo, Estados ufCrm)
    {
        IdMedico = IdUsuario;
        Especialidade = especialidade;
        CrmNumber = crmNumber;
        UfCrm = ufCrm;
        Ativo = ativo ?? true;
    }
    
    public bool SoftDelete()
    {
        if(!Ativo) 
            return false;
        
        Ativo = false;
        return true;
    }
}