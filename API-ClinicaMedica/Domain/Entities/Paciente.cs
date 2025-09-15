using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API_ClinicaMedica.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace API_ClinicaMedica.Domain.Entities;
[Table("Pacientes")]
public class Paciente
{
    [Key]
    public int IdPaciente { get; private set; }
    public Usuario Usuario { get; private set; }
    public bool Pcd { get; private set; }
    public bool Ativo { get; private set; }

    public IReadOnlyCollection<MedicamentoControlado> MedicamentosControlados { get; private set; }
    protected Paciente()
    {
        
    }
    
    public Paciente(int idPaciente, bool pcd, List<MedicamentoControlado> medicamentosControlados)
    {
        IdPaciente = idPaciente;
        Pcd = pcd;
        Ativo = true;
        MedicamentosControlados = medicamentosControlados ?? new List<MedicamentoControlado>();
    }

    public void setPacienteId(int id)
    {
        IdPaciente = id;
    }
    public void setAtivoTrue()
    {
        Ativo = true;
    }
    public bool Desativar()
    {
        if(!Ativo) 
            return false;
        
        Ativo = false;
        return true;
    }
    
    
}