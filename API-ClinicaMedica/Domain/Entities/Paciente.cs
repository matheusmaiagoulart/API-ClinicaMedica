using System.ComponentModel.DataAnnotations;
using API_ClinicaMedica.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace API_ClinicaMedica.Domain.Entities;

public class Paciente
{
    [Key]
    public int Id { get; set; }
    public Usuario Usuario { get; set; }
    public bool Pcd { get; private set; }
    public bool Ativo { get; private set; }


    protected Paciente()
    {
        
    }
    
    public Paciente(Usuario usuario, bool ativo, bool pcd)
    {
        Usuario = usuario;
        Pcd = pcd;
        Ativo = true;
    }
    
    public void Desativar()
    {
        Ativo = false;
    }
    
    
}