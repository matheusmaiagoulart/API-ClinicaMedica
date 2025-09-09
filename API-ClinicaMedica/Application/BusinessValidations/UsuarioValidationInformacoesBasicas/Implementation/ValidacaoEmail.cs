using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Infra.Interfaces;

namespace API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Implementation;

public class ValidacaoEmail : IValidacaoInformacoesBasicas
{
    private readonly IUnitOfWork _unitOfWork;
    public ValidacaoEmail(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Validacao(UniqueFieldsValidationDTO dto)
    {
        var email = dto.Email;
        if ( dto.IdUsuario > 0)
        {
            //Validação com ID valido, cai quando for atualização de algum usuário
            var userObj = await _unitOfWork.Usuarios.GetUserById(dto.IdUsuario);
            if (userObj  == null)
            {
                return Result.Failure(UsuarioErrosResults.UsuarioNaoEncontrado());
            }
            if (userObj.Email != dto.Email)
            {
                
                //Validação de disponibilidade do email no banco de dados
                var isEmailAvailable = await _unitOfWork.Usuarios.isEmailAvailable(dto.Email);
                if (isEmailAvailable == false)
                    return Result.Failure(UsuarioErrosResults.EmailJaCadastrado(dto.Email));
            }
            return Result.Success();
        }
        //Validação pra usuários novos, sem ID


            var EmailAvailable = await _unitOfWork.Usuarios.isEmailAvailable(dto.Email);

            if (EmailAvailable == false)
                return Result.Failure(UsuarioErrosResults.EmailJaCadastrado(dto.Email));

            return Result.Success();
    }


}