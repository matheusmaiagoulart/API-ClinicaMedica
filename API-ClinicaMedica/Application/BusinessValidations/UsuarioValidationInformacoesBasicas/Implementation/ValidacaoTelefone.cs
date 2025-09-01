using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Infra.Interfaces;

namespace API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Implementation;

public class ValidacaoTelefone : IValidacaoInformacoesBasicas
{
    private readonly IUnitOfWork _unitOfWork;

    public ValidacaoTelefone(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Validacao(UniqueFieldsValidationDTO dto)
    {
        var telefone = dto.InformacoesBasicas.Telefone;

        if (dto.IdUsuario != null && dto.IdUsuario > 0)
        {
            //Validação com ID valido, cai quando for atualização de algum usuário
            var user = await _unitOfWork.Usuarios.GetUserById(dto.IdUsuario);
            if (user == null)
            {
                return Result.Failure(UsuarioErrosResults.UsuarioNaoEncontrado());
            }

            if (user.InformacoesBasicas.Telefone != dto.InformacoesBasicas.Telefone)
            {
                //Validação de disponbibilidade do telefone no banco de dados

                var isTelefoneAvailable = await _unitOfWork.Usuarios.isTelefoneAvailable(telefone);

                if (isTelefoneAvailable == false)
                    Result.Failure(UsuarioErrosResults.TelefoneJaCadastrado(telefone));

            }
        }
        var telefoneAvalialble = await _unitOfWork.Usuarios.isTelefoneAvailable(telefone);
        if (telefoneAvalialble == false)
        {
            return Result.Failure(UsuarioErrosResults.TelefoneJaCadastrado(telefone));
        }

        return Result.Success();


    }
}