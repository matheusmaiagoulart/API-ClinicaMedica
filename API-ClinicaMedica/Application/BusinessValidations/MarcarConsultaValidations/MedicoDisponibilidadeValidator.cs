using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Results.EntitiesResults;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Infra.Interfaces;

namespace API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;

public class MedicoDisponibilidadeValidator : IMarcarConsultaValidator
{
    private readonly IUnitOfWork _unitOfWork;

    public MedicoDisponibilidadeValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Validacao(CreateConsultaDTO dto)
    {
        var medicoId = dto.IdMedico;
        var especialidade = dto.Especialidade;
        var dataConsulta = dto.DataHoraConsulta;

        if (medicoId == 0)
        {
            //Buscar por medico aleatorio

            var medicoAleatorioDisponivel = await _unitOfWork.Consultas.EscolheMedicoAleatorio(dataConsulta, especialidade);
            var medicoAleatorioDisponivel =
                await _unitOfWork.Consultas.EscolheMedicoAleatorio(dataConsulta, especialidade);
            if (medicoAleatorioDisponivel == null)
                return Result.Failure(ConsultaErrosResult.NenhumMedicoDisponivel());

            dto.IdMedico = medicoAleatorioDisponivel.IdMedico;
            return Result.Success();
        }


        var disponivel = await _unitOfWork.Consultas.VerificarDisponibilidadeConsulta(medicoId, dataConsulta);
        if (!disponivel)
        {
            return Result.Failure(ConsultaErrosResult.MedicoNaoDisponivelNaDataEscolhida());
        }

        var conferirEspecialidadeMedico = await _unitOfWork.Medicos.GetEspecialidadeById(medicoId);
        if (conferirEspecialidadeMedico != especialidade)
        {
            return Result.Failure(MedicoErrosResult.MedicoNaoPossuiEspecialidade());
        }

        return Result.Success();
    }
}