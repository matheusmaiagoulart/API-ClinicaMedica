using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.ValueObjects;

namespace API_ClinicaMedica.Application.DTOs.PacienteDTOs;

public class UpdatePacienteDTO
{
    public int IdPaciente { get; set; }
    public bool Pcd { get; set; }
    
    public bool Ativo { get; set; }
    public IReadOnlyCollection<MedicamentoControlado> MedicamentosControlados { get; set; }
}