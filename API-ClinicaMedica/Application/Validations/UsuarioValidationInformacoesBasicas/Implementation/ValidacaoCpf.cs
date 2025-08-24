
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
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

    public async Task validacao(UniqueFieldsValidationDTO dto)
    {
        var user = await _unitOfWork.Usuarios.GetUserById(dto.IdUsuario);
        if (user == null)
        {
            throw new UsuarioNaoEncontradoException("Usuário não localizado!");
        }
        if (user.InformacoesBasicas.Cpf != dto.InformacoesBasicas.Cpf)
        {
            //Validação da string do CPF da entrada
            var cpf = dto.InformacoesBasicas.Cpf;
        
            var cpfIsAvailable = await _unitOfWork.Usuarios.isCpfAvailable(cpf);
            
                if(cpfIsAvailable == false)
                    throw new CpfException($"{dto.InformacoesBasicas.Cpf}");
        }
    

        
        
    }
}