using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Infra.Interfaces;

namespace API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;

public class MedicoValidoValidator : IMarcarConsultaValidator
{
    private readonly IUnitOfWork _unitOfWork;
    public MedicoValidoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Validacao(CreateConsultaDTO dto)
    {
        var medico = await _unitOfWork.Medicos.GetMedicoById(dto.IdMedico);
        
        if (medico == null)
            return Result.Failure(MedicoErrosResult.MedicoNaoEncontrado());
        
        if(!medico.Ativo)
            return Result.Failure(MedicoErrosResult.MedicoInativo());
        
        return Result.Success();
    }
}