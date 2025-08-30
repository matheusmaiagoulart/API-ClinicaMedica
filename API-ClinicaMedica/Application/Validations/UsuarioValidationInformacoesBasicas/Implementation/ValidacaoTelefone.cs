
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Application.Results.UsuariosResults;
using API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Infra.Exceptions;
using API_ClinicaMedica.Infra.Repositories.UnitOfWork;

namespace API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Implementation;

public class ValidacaoTelefone : IValidacaoInformacoesBasicas
{
    private readonly IUnitOfWork _unitOfWork;

    public ValidacaoTelefone(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> validacao(UniqueFieldsValidationDTO dto)
    {
        var telefone = dto.InformacoesBasicas.Telefone;

        if (dto.IdUsuario != null && dto.IdUsuario > 0)
        {
            //Validação com ID valido, cai quando for atualização de algum usuário
            var user = await _unitOfWork.Usuarios.GetUserById(dto.IdUsuario);
            if (user == null)
            {
                return Result.Failure(UsuariosErrosResults.UsuarioNaoEncontrado());
            }

            if (user.InformacoesBasicas.Telefone != dto.InformacoesBasicas.Telefone)
            {
                //Validação de disponbibilidade do telefone no banco de dados

                var isTelefoneAvailable = await _unitOfWork.Usuarios.isTelefoneAvailable(telefone);

                if (isTelefoneAvailable == false)
                    Result.Failure(UsuariosErrosResults.TelefoneJaCadastrado(telefone));

            }
        }
        var telefoneAvalialble = await _unitOfWork.Usuarios.isTelefoneAvailable(telefone);
        if (telefoneAvalialble == false)
        {
            return Result.Failure(UsuariosErrosResults.TelefoneJaCadastrado(telefone));
        }

        return Result.Success();


    }
}