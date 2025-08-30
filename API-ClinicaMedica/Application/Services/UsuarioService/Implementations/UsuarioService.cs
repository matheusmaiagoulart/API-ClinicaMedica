using System.Runtime.InteropServices.JavaScript;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Application.Results.UsuariosResults;
using API_ClinicaMedica.Application.Services.UsuarioService.Interfaces;
using API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Infra.Exceptions;
using API_ClinicaMedica.Infra.Repositories.UnitOfWork;
using AutoMapper;
using FluentValidation;


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

    public async Task<Result<Usuario>> CreateUser(CreateUsuarioDTO dto)
    {
        var validation = _mapper.Map<UniqueFieldsValidationDTO>(dto);
        foreach (var index in validacaoInformacoesBasicas)
        {
            var result = index.validacao(validation);
            if (result.Result.IsFailure)
            {
                return Result<Usuario>.Failure(result.Result.Error);
            }
        }


        Usuario user = _mapper.Map<Usuario>(dto);
        user.HashSenha(dto.Senha);
        
        _unitOfWork.Usuarios.AddAsync(user);
        await _unitOfWork.CommitAsync();
        
        return Result<Usuario>.Success(user);

    }

    public async Task<Result<UsuarioDTO>> GetUserById(int id)
    {
        var user = await _unitOfWork.Usuarios.GetUserById(id);
        if (user == null)
        {
            return Result<UsuarioDTO>.Failure(UsuariosErrosResults.UsuarioNaoEncontrado());
        }
        var userDTO = _mapper.Map<UsuarioDTO>(user);
        return Result<UsuarioDTO>.Success(userDTO);
    }

    public async Task<Result<IEnumerable<UsuarioDTO>>> GetAllUsers()
    {
        var allUsers = await _unitOfWork.Usuarios.GetAllUsers();
        if (allUsers == null)
        {
            return Result<IEnumerable<UsuarioDTO>>.Failure(UsuariosErrosResults.UsuariosNaoEncontrado());
        }
        var lista = _mapper.Map<IEnumerable<UsuarioDTO>>(allUsers);
        
        return Result<IEnumerable<UsuarioDTO>>.Success(lista);
    }

    public async Task<Result<UsuarioDTO>> UpdateUser(int id, UpdateUsuarioDTO dto)
    {
        var userExistente = await _unitOfWork.Usuarios.GetUserById(id);
        if (userExistente == null)
        {
            return Result<UsuarioDTO>.Failure(UsuariosErrosResults.UsuarioNaoEncontrado());
        }
        _mapper.Map(dto, userExistente);
        
        var validation = _mapper.Map<UniqueFieldsValidationDTO>(userExistente);
        foreach (var index in validacaoInformacoesBasicas)
            {
                    index.validacao(validation);
            }
        
        await _unitOfWork.CommitAsync();
        
        var userDTO = _mapper.Map<UsuarioDTO>(userExistente);
        return Result<UsuarioDTO>.Success(userDTO);
    }
    
}

   