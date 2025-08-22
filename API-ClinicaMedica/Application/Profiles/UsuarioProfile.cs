using API_ClinicaMedica.Domain.DTOs;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.ValueObjects;
using AutoMapper;

namespace API_ClinicaMedica.Application.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDTO, Usuario>().ConstructUsing(
            dto => new Usuario
            (
                dto.Email,
                dto.Senha,
                new InformacoesBasicas(
                    dto.InformacoesBasicas.Nome,
                    dto.InformacoesBasicas.Telefone,
                    dto.InformacoesBasicas.DataNascimento,
                    dto.InformacoesBasicas.Cpf,
                    dto.InformacoesBasicas.Rg)
                , new Endereco(
                    dto.Endereco.Logradouro,
                    dto.Endereco.Numero,
                    dto.Endereco.Complemento,
                    dto.Endereco.Bairro,
                    dto.Endereco.Cidade,
                    dto.Endereco.Estado,
                    dto.Endereco.Cep
                ))
            );

        CreateMap<Usuario, UsuarioDTO>();


    }
    
}