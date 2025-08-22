using API_ClinicaMedica.Application.Validations.Interface.UsuarioValidationInformacoesBasicas;
using API_ClinicaMedica.Domain.DTOs;
using API_ClinicaMedica.Infra.Exceptions;
using API_ClinicaMedica.Repositories.UnitOfWork;

namespace API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Implementation;

public class ValidacaoEmail : IValidacaoInformacoesBasicas
{
    private readonly IUnitOfWork _unitOfWork;
    public ValidacaoEmail(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void validacao(CreateUsuarioDTO dto)
    {
        
        //Validação da string do email de entrada    
        if (string.IsNullOrEmpty(dto.Email) || !dto.Email.Contains("@"))
            throw new ArgumentException("Email inválido.");
        
        
        //Validação de disponibilidade do email no banco de dados
        
        var isEmailAvailable=  _unitOfWork.Usuarios.isEmailAvailable(dto.Email);
       
        if (isEmailAvailable.Result == false)
            throw new EmailException(dto.Email);
        
    }
}