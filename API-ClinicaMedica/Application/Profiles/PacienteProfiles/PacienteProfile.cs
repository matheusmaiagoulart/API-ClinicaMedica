using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Domain.Entities;
using AutoMapper;

namespace API_ClinicaMedica.Application.Profiles.PacienteProfiles;

public class PacienteProfile : Profile
{
    public PacienteProfile()
    {
        CreateMap<CreatePacienteDTO, Paciente>();
        
        CreateMap<Paciente, PacienteDTO>();

        CreateMap<IEnumerable<Paciente>, IEnumerable<PacienteDTO>>();

        CreateMap<UpdatePacienteDTO, Paciente>();
    }
    
}