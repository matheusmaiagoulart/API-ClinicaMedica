using API_ClinicaMedica.Domain.Enums;

namespace API_ClinicaMedica.Application.DTOs.MedicoDTOs;

public class CreateMedicoDTO
{
    public int IdUsuario { get; set; }
    public string Especialidade { get;  set;}
    public string CrmNumber { get; set;}
    public string UfCrm { get; set;}
    public bool Ativo { get; set; }
}