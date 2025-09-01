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
    
    public Paciente(int IdPaciente, Usuario usuario, bool ativo, bool pcd, List<MedicamentoControlado> medicamentosControlados)
    {
        this.IdPaciente = IdPaciente;
        Usuario = usuario;
        Pcd = pcd;
        Ativo = ativo;
        this.MedicamentosControlados = medicamentosControlados ?? new List<MedicamentoControlado>();
    }

    public void setAtivoTrue()
    {
        Ativo = true;
    }
    public void Desativar()
    {
        Ativo = false;
    }
    
    
}