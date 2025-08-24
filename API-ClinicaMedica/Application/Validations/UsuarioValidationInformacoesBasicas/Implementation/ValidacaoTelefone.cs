
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;
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
    
    public async Task validacao(UniqueFieldsValidationDTO dto)
    {
        var user = await _unitOfWork.Usuarios.GetUserById(dto.IdUsuario);
        if (user == null)
        {
            throw new UsuarioNaoEncontradoException("Usuário não localizado!");
        }

        if (user.InformacoesBasicas.Telefone != dto.InformacoesBasicas.Telefone)
        {
            //Validação de disponbibilidade do telefone no banco de dados
            var telefone = dto.InformacoesBasicas.Telefone;
            var isTelefoneAvailable=  await _unitOfWork.Usuarios.isTelefoneAvailable(telefone);
        
            if(isTelefoneAvailable == false)
                throw new TelefoneException(telefone);
        
        }
    }
        
}