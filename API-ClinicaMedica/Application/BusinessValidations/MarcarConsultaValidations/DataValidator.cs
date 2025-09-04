using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Infra.Interfaces;

namespace API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;

public class DataValidator : IMarcarConsultaValidator
{
    private readonly IUnitOfWork _unitOfWork;
    public DataValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Validacao(CreateConsultaDTO dto)
    {
        var dataConsulta = dto.DataHoraConsulta;
        var now = DateTime.Now;

        
        if(dataConsulta < now)
            return Result.Failure(ConsultaErrosResult.DataInvalidaMenorQueAtual());

        
        if (dataConsulta < now.AddHours(1))
            return Result.Failure(ConsultaErrosResult.AntecedenciaMinimaNecessaria());
        
        
        if(dataConsulta > now.AddMonths(3))
            return Result.Failure(ConsultaErrosResult.DataInvalidaMaiorTresMeses());
        
        return Result.Success();
    }
}