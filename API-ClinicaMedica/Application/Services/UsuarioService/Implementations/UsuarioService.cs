using System.Runtime.InteropServices.JavaScript;
using API_ClinicaMedica.Application.DTOs.UpdateUsuarioDTOs;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Services.UsuarioService.Interfaces;
using API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Infra.Exceptions;
using API_ClinicaMedica.Infra.Repositories.UnitOfWork;
using AutoMapper;


namespace API_ClinicaMedica.Application.Services.UsuarioService.Implementations;

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private List<IValidacaoInformacoesBasicas> validacaoInformacoesBasicas;

    public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper, IEnumerable<IValidacaoInformacoesBasicas> validacoes)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        validacaoInformacoesBasicas = validacoes.ToList();
    }

    public async Task<Usuario> CreateUser(CreateUsuarioDTO dto)
    {
        var validation = _mapper.Map<UniqueFieldsValidationDTO>(dto);
        foreach (var index in validacaoInformacoesBasicas)
        {
            index.validacao(validation);
        }

        Usuario user = _mapper.Map<Usuario>(dto);
        _unitOfWork.Usuarios.AddAsync(user);
        await _unitOfWork.CommitAsync();
        return user;

    }

    public async Task<UsuarioDTO> GetUserById(int id)
    {
        var user = await _unitOfWork.Usuarios.GetUserById(id);
        if (user == null)
        {
            throw new UsuarioNaoEncontradoException("Usuário não localizado!");
        }
        var userDTO = _mapper.Map<UsuarioDTO>(user);
        return userDTO;
    }

    public async Task<IEnumerable<UsuarioDTO>> GetAllUsers()
    {
        var allUsers = await _unitOfWork.Usuarios.GetAllUsers();
        var lista = _mapper.Map<IEnumerable<UsuarioDTO>>(allUsers);
        return lista;
    }

    public async Task<UsuarioDTO> UpdateUser(int id, UpdateUsuarioDTO dto)
    {
        var userExistente = await _unitOfWork.Usuarios.GetUserById(id);
        _mapper.Map(dto, userExistente);
        
        var validation = _mapper.Map<UniqueFieldsValidationDTO>(userExistente);
        foreach (var index in validacaoInformacoesBasicas)
            {
                    index.validacao(validation);
            }
                
        
         
        
        
        // var usuarioAtualizado = new Usuario(
        //     idUsuario : userExistente.IdUsuario,
        //     email : dto.Email.IsNullOrEmpty() ? userExistente.Email : dto.Email,
        //     senha : dto.Senha.IsNullOrEmpty() ? userExistente.Senha : dto.Senha,
        //     infos : dto.InformacoesBasicas != null ? new InformacoesBasicas
        //     (
        //         nome : dto.InformacoesBasicas.Nome.IsNullOrEmpty() ? userExistente.InformacoesBasicas.Nome : dto.InformacoesBasicas.Nome,
        //         telefone : dto.InformacoesBasicas.Telefone.IsNullOrEmpty() ? userExistente.InformacoesBasicas.Telefone : dto.InformacoesBasicas.Telefone,
        //         cpf : dto.InformacoesBasicas.Cpf.IsNullOrEmpty() ? userExistente.InformacoesBasicas.Cpf : dto.InformacoesBasicas.Cpf,
        //         rg : dto.InformacoesBasicas.Rg.IsNullOrEmpty() ? userExistente.InformacoesBasicas.Rg : dto.InformacoesBasicas.Rg,
        //         dataNascimento : dto.InformacoesBasicas.DataNascimento.HasValue && dto.InformacoesBasicas.DataNascimento != userExistente.InformacoesBasicas.DataNascimento
        //             ? dto.InformacoesBasicas.DataNascimento.Value
        //             : userExistente.InformacoesBasicas.DataNascimento
        //     ) : userExistente.InformacoesBasicas,
        //     
        //     endereco : dto.Endereco == null ? new Endereco
        //     (
        //         logradouro: dto.Endereco.Logradouro.IsNullOrEmpty() ? userExistente.Endereco.Logradouro : dto.Endereco.Logradouro,
        //         numero: dto.Endereco.Numero.IsNullOrEmpty() ? userExistente.Endereco.Numero : dto.Endereco.Numero,
        //         complemento: dto.Endereco.Complemento.IsNullOrEmpty() ? userExistente.Endereco.Complemento : dto.Endereco.Complemento,
        //         bairro: dto.Endereco.Bairro.IsNullOrEmpty() ? userExistente.Endereco.Bairro : dto.Endereco.Bairro,
        //         cidade: dto.Endereco.Cidade.IsNullOrEmpty() ? userExistente.Endereco.Cidade : dto.Endereco.Cidade,
        //         estado: !dto.Endereco.Estado.HasValue ? userExistente.Endereco.Estado : dto.Endereco.Estado.Value,
        //         cep: dto.Endereco.Cep.IsNullOrEmpty() ? userExistente.Endereco.Cep : dto.Endereco.Cep
        //         ) : userExistente.Endereco);
        
        
        await _unitOfWork.CommitAsync();
        
        var userDTO = _mapper.Map<UsuarioDTO>(userExistente);
        return userDTO;
    }
    
}

   