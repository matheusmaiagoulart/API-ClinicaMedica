using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Interfaces;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Infra.Interfaces;
using AutoMapper;

namespace API_ClinicaMedica.Application.Services;

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
            var result = index.Validacao(validation);
            if (result.Result.IsFailure)
            {
                return Result<Usuario>.Failure(result.Result.Error);
            }
        }


        Usuario user = _mapper.Map<Usuario>(dto);
        var criptResult = user.HashSenha(dto.Senha);
        if (!criptResult)
        {
            return Result<Usuario>.Failure(UsuarioErrosResults.ErroAoCriptografarSenha());
        }
        
        await _unitOfWork.Usuarios.AddAsync(user);
        await _unitOfWork.CommitAsync();
        
        return Result<Usuario>.Success(user);

    }

    public async Task<Result<UsuarioDTO>> GetUserById(int id)
    {
        var user = await _unitOfWork.Usuarios.GetUserById(id);
        if (user == null)
        {
            return Result<UsuarioDTO>.Failure(UsuarioErrosResults.UsuarioNaoEncontrado());
        }
        var userDTO = _mapper.Map<UsuarioDTO>(user);
        return Result<UsuarioDTO>.Success(userDTO);
    }

    public async Task<Result<IEnumerable<UsuarioDTO>>> GetAllUsers()
    {
        var allUsers = await _unitOfWork.Usuarios.GetAllUsers();
        if (allUsers == null)
        {
            return Result<IEnumerable<UsuarioDTO>>.Failure(UsuarioErrosResults.UsuariosNaoEncontrado());
        }
        var lista = _mapper.Map<IEnumerable<UsuarioDTO>>(allUsers);
        
        return Result<IEnumerable<UsuarioDTO>>.Success(lista);
    }

    public async Task<Result<UsuarioDTO>> UpdateUser(int id, UpdateUsuarioDTO dto)
    {
        var userExistente = await _unitOfWork.Usuarios.GetUserById(id);
        if (userExistente == null)
        {
            return Result<UsuarioDTO>.Failure(UsuarioErrosResults.UsuarioNaoEncontrado());
        }
       // _mapper.Map(dto, userExistente);
       userExistente  = _mapper.Map<UpdateUsuarioDTO, Usuario>(dto);
        var validation = _mapper.Map<UniqueFieldsValidationDTO>(userExistente);
        foreach (var index in validacaoInformacoesBasicas)
            {
                    var result = await index.Validacao(validation);
                    if (result.IsFailure)
                    {
                        return Result<UsuarioDTO>.Failure(result.Error);
                    }
            }
        await _unitOfWork.Usuarios.UpdateAsync(userExistente);
        await _unitOfWork.CommitAsync();
        
        var userDTO = _mapper.Map<UsuarioDTO>(userExistente);
        return Result<UsuarioDTO>.Success(userDTO);
    }
    
}

   