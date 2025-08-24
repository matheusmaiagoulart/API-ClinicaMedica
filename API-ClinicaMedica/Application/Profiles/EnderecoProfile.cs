using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.EnderecoDTOs;
using API_ClinicaMedica.Domain.ValueObjects;
using AutoMapper;

namespace API_ClinicaMedica.Application.Profiles
{
    public class EnderecoProfile : Profile
    {
        public EnderecoProfile()
        {
            CreateMap<EnderecoDTO, Endereco>();

            CreateMap<EnderecoUpdateDTO, Endereco>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                srcMember != null ));
            
            CreateMap<Endereco, EnderecoDTO>();
            CreateMap<Endereco, EnderecoUpdateDTO>();
            

        }
    }
}
