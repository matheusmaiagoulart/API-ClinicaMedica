using API_ClinicaMedica.Application.Validations.Interface.UsuarioValidationInformacoesBasicas;
using API_ClinicaMedica.Domain.DTOs;
using API_ClinicaMedica.Infra.Exceptions;
using API_ClinicaMedica.Repositories.UnitOfWork;

namespace API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Implementation;

public class ValidacaoCpf : IValidacaoInformacoesBasicas
{
    private readonly IUnitOfWork _unitOfWork;
    public ValidacaoCpf(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void validacao(CreateUsuarioDTO dto)
    {
        //Validação da string do CPF da entrada
        var cpf = dto.InformacoesBasicas.Cpf;
        
        var cpfIsAvailable = _unitOfWork.Usuarios.isCpfAvailable(cpf);
        
        if(cpfIsAvailable.Result == false)
            throw new CpfException($"{dto.InformacoesBasicas.Cpf}");
    }
}