
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results;
using API_ClinicaMedica.Application.Results.UsuariosResults;
using API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Infra.Exceptions;
using API_ClinicaMedica.Infra.Repositories.UnitOfWork;

namespace API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Implementation;

public class ValidacaoCpf : IValidacaoInformacoesBasicas
{
    private readonly IUnitOfWork _unitOfWork;
    public ValidacaoCpf(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> validacao(UniqueFieldsValidationDTO dto)
    {
        var cpfUser = dto.InformacoesBasicas.Cpf;

        if (dto.IdUsuario != null && dto.IdUsuario > 0)
        {
            var user = await _unitOfWork.Usuarios.GetUserById(dto.IdUsuario);

            if (user == null)
            {
                return Result.Failure(UsuariosErrosResults.UsuarioNaoEncontrado());
            }

            if (user.InformacoesBasicas.Cpf != dto.InformacoesBasicas.Cpf)
            {

                var cpfIsAvailableUpdate = await _unitOfWork.Usuarios.isCpfAvailable(cpfUser);

                if (cpfIsAvailableUpdate == false)
                    return Result.Failure(UsuariosErrosResults.CpfJaCadastrado(cpfUser));
            }

            return Result.Success();
        }
        
           
            var cpf = dto.InformacoesBasicas.Cpf;

            var cpfIsAvailable = await _unitOfWork.Usuarios.isCpfAvailable(cpf);
            if (cpfIsAvailable == false)
                return Result.Failure(UsuariosErrosResults.CpfJaCadastrado(cpf));

            return Result.Success();

    }
}