using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Domain.Entities;
using AutoMapper;

namespace API_ClinicaMedica.Application.Profiles.EntitiesProfiles;

public class MedicoProfile : Profile
{
    public MedicoProfile()
    {
        CreateMap<CreateMedicoDTO, Medico>()
            .ForMember(m => m.IdMedico, opt => opt.MapFrom(dto => dto.IdUsuario));
        CreateMap<Medico, MedicoDTO>();
        
        
    }
    
}