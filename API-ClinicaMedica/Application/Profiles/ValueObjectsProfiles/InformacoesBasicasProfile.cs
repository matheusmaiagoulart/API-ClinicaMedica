using API_ClinicaMedica.Application.DTOs.ValueObjectsDTOs.InformacoesBasicasDTOs;
using API_ClinicaMedica.Domain.ValueObjects;
using AutoMapper;

namespace API_ClinicaMedica.Application.Profiles.ValueObjectsProfiles
{
    public class InformacoesBasicasProfile  : Profile
    {

        public InformacoesBasicasProfile()
        {
            
            CreateMap<InformacoesBasicas, InformacoesBasicasDTO>();
            CreateMap<InformacoesBasicas, InformacoesBasicasFieldValidationDTO>();
            
            
            CreateMap<InformacoesBasicasDTO, InformacoesBasicas>();
            CreateMap<InformacoesBasicasDTO, InformacoesBasicasFieldValidationDTO>();

            
            CreateMap<InformacoesBasicasUpdateDTO, InformacoesBasicas>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                    srcMember != null));
            
            CreateMap<InformacoesBasicasFieldValidationDTO, InformacoesBasicas>();
            
            
            
            

        }
    }
}
