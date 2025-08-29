using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.ValueObjects;

namespace API_ClinicaMedica.Application.DTOs.PacienteDTOs;

public class PacienteDTO
{
    public int IdPaciente { get; set; }
    public Usuario Usuario { get; set; }
    public bool Pcd { get; private set; }
    public bool Ativo { get; private set; }
    public IReadOnlyCollection<MedicamentoControlado> MedicamentosControlados { get; private set; }
}