using API_ClinicaMedica.Domain.ValueObjects;

namespace API_ClinicaMedica.Application.DTOs.PacienteDTOs;

public class CreatePacienteDTO
{
    public int IdUsuario { get; set; }
    public bool Pcd { get; set; }

    public IReadOnlyCollection<MedicamentoControlado> MedicamentosControlados { get; set; }
}