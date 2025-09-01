using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.DTOs.MedicoDTOs;

public class MedicoDTO
{
    public int IdMedico { get; set; }
    public Usuario Usuario { get; set; }
    public Especialidades Especialidade { get; set;}
    public Estados UfCrm { get; set; }
    public string CrmNumber { get; set;}
    public bool Ativo { get; set; }
}