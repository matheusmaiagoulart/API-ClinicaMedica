using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API_ClinicaMedica.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace API_ClinicaMedica.Domain.Entities;
[Table("Pacientes")]
public class Paciente
{
    [Key]
    public int IdPaciente { get; set; }
    public Usuario Usuario { get; set; }
    public bool Pcd { get; private set; }
    public bool Ativo { get; private set; }

    public IReadOnlyCollection<MedicamentoControlado> MedicamentosControlados { get; private set; }
    protected Paciente()
    {
        
    }
    
    public Paciente(Usuario usuario, bool ativo, bool pcd, List<MedicamentoControlado> medicamentosControlados)
    {
        Usuario = usuario;
        Pcd = pcd;
        Ativo = true;
        this.MedicamentosControlados = medicamentosControlados ?? new List<MedicamentoControlado>();
    }
    
    
    public void Desativar()
    {
        Ativo = false;
    }
    
    
}