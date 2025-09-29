using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Domain.Entities;
using AutoMapper;

namespace API_ClinicaMedica.Application.Profiles.EntitiesProfiles;

public class PacienteProfile : Profile
{
    public PacienteProfile()
    {
        CreateMap<CreatePacienteDTO, Paciente>()
            .ForMember(v => v.IdPaciente, src => src.MapFrom(dto => dto.IdUsuario));
        
        CreateMap<Paciente, PacienteDTO>();

        CreateMap<UpdatePacienteDTO, Paciente>();
    }
    
}