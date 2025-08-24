
using System.Xml;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Infra.Exceptions;
using API_ClinicaMedica.Infra.Repositories.UnitOfWork;

namespace API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Implementation;

public class ValidacaoEmail : IValidacaoInformacoesBasicas
{
    private readonly IUnitOfWork _unitOfWork;
    public ValidacaoEmail(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task validacao(UniqueFieldsValidationDTO dto)
    {

        var user = await _unitOfWork.Usuarios.GetUserById(dto.IdUsuario);
        if (user == null)
        {
            throw new UsuarioNaoEncontradoException("Usuário não localizado!");
        }

        if (user.Email != dto.Email)
        {
            //Validação da string do email de entrada    
            if (string.IsNullOrEmpty(dto.Email) || !dto.Email.Contains("@"))
                throw new ArgumentException("Email inválido.");


            //Validação de disponibilidade do email no banco de dados

            var isEmailAvailable = await _unitOfWork.Usuarios.isEmailAvailable(dto.Email);

            if (isEmailAvailable == false)
                throw new EmailException(dto.Email);

        }
    }


}