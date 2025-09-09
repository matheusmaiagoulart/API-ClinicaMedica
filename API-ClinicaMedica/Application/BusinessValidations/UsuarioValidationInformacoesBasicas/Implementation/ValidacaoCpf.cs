using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Infra.Interfaces;

namespace API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Implementation;

public class ValidacaoCpf : IValidacaoInformacoesBasicas
{
    private readonly IUnitOfWork _unitOfWork;
    public ValidacaoCpf(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Validacao(UniqueFieldsValidationDTO dto)
    {
        var cpfUser = dto.InformacoesBasicas.Cpf;

        if (dto.IdUsuario > 0)
        {
            var user = await _unitOfWork.Usuarios.GetUserById(dto.IdUsuario);

            if (user == null)
            {
                return Result.Failure(UsuarioErrosResults.UsuarioNaoEncontrado());
            }

            if (user.InformacoesBasicas.Cpf != dto.InformacoesBasicas.Cpf)
            {

                var cpfIsAvailableUpdate = await _unitOfWork.Usuarios.isCpfAvailable(cpfUser);

                if (cpfIsAvailableUpdate == false)
                    return Result.Failure(UsuarioErrosResults.CpfJaCadastrado(cpfUser));
            }

            return Result.Success();
        }
        
           
            var cpf = dto.InformacoesBasicas.Cpf;

            var cpfIsAvailable = await _unitOfWork.Usuarios.isCpfAvailable(cpf);
            if (cpfIsAvailable == false)
                return Result.Failure(UsuarioErrosResults.CpfJaCadastrado(cpf));

            return Result.Success();

    }
}