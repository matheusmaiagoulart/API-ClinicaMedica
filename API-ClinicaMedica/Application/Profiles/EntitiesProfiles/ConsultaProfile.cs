using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.ValueObjects;
using AutoMapper;

namespace API_ClinicaMedica.Application.Profiles.EntitiesProfiles;

public class ConsultaProfile : Profile
{
    public ConsultaProfile()
    {
        CreateMap<Consulta, ConsultaViewDTO>()
            .ForMember(dest => dest.Paciente, opt => opt.MapFrom(src => src.Paciente))
            .ForMember(dest => dest.Medico, opt => opt.MapFrom(src => src.Medico));


        CreateMap<Paciente, PacienteConsultaViewDTO>();

        CreateMap<Medico, MedicoConsultaViewDTO>();

        CreateMap<Usuario, UsuarioConsultaViewDTO>();
        
        CreateMap<InformacoesBasicas, InformacoesBasicasConsultaViewDTO>();
        

        CreateMap<CreateConsultaDTO, Consulta>();
        
        CreateMap<Consulta, ConsultaDTO>();

    }
}
