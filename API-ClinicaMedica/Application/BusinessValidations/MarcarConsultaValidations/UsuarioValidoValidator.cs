using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Infra.Interfaces;

namespace API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;

public class UsuarioValidoValidator : IMarcarConsultaValidator
{
    private readonly IUnitOfWork _unitOfWork;
    public UsuarioValidoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Validacao(CreateConsultaDTO dto)
    {
        var paciente = await _unitOfWork.Pacientes.GetPacienteById(dto.IdPaciente);
        if (paciente == null)
            return Result.Failure(UsuarioErrosResults.UsuarioNaoEncontrado());
        
        if(!paciente.Ativo)
            return Result.Failure(PacienteErrorsResult.PacienteInativo());
        
        return Result.Success();
    }
}