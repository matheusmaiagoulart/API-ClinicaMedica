using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Application.FluentValidation.MedicoValidations;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Application.Results.MedicosResults;
using API_ClinicaMedica.Application.Results.UsuariosResults;
using API_ClinicaMedica.Application.Services.MedicoService.Interfaces;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Domain.Enums;
using API_ClinicaMedica.Infra.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace API_ClinicaMedica.Application.Services.MedicoService.Implementations;

public class MedicoService : IMedicoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public MedicoService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
        
    }
    
    public async Task<Result<Medico>> CreateMedico(CreateMedicoDTO dto)
    {
        var usuario = await _uow.Usuarios.existsById(dto.IdUsuario);
        if (!usuario)
            return Result<Medico>.Failure(UsuariosErrosResults.UsuarioNaoEncontrado());
        
        var jaVinculado = await _uow.Medicos.existsMedicoById(dto.IdUsuario);
        if (jaVinculado)
            return Result<Medico>.Failure(MedicosErrosResult.IdJaVinculadoAUsuario());
        
        var CRMExists = await _uow.Medicos.CRMExists(dto.CrmNumber);
        if (CRMExists)
        {
            return Result<Medico>.Failure(MedicosErrosResult.CrmJaExistenteNoDB());
        }
        
        var newMedico = _mapper.Map<Medico>(dto);
        

        await _uow.Medicos.AddAsync(newMedico);
        
        await _uow.CommitAsync();
        return Result<Medico>.Success(newMedico);

    }

    public async Task<Result<MedicoDTO>> GetMedicoById(int id)
    {
        var medico = await _uow.Medicos.GetMedicoById(id);
        if (medico == null)
            return Result<MedicoDTO>.Failure(MedicosErrosResult.MedicoNaoEncontrado());
        
        var medicoDTO = _mapper.Map<MedicoDTO>(medico);
        
        return Result<MedicoDTO>.Success(medicoDTO);
    }

    public async Task<Result<IEnumerable<MedicoDTO>>> GetAllActiveMedicos()
    {
        var allActiveMedicos = await _uow.Medicos.GetAllActiveMedicos();
        if(allActiveMedicos == null)
            return Result<IEnumerable<MedicoDTO>>.Failure(MedicosErrosResult.MedicosAtivosNaoEncontrados());
        
        var allActiveMedicosDTO = _mapper.Map<IEnumerable<MedicoDTO>>(allActiveMedicos);
        
        return Result<IEnumerable<MedicoDTO>>.Success(allActiveMedicosDTO);
    }

    public async Task<Result<IEnumerable<MedicoDTO>>> GetMedicosByEspecialidadeAndActiveTrue(Especialidades e)
    {
        var medicos = await _uow.Medicos.GetMedicosByEspecialidadeAndActiveTrue(e);
        if(medicos.IsNullOrEmpty())
            return Result<IEnumerable<MedicoDTO>>.Failure(MedicosErrosResult.NenhumMedicoParaEssaEspecialidade());
        
        var medicosDTO = _mapper.Map<IEnumerable<MedicoDTO>>(medicos);
        
        return Result<IEnumerable<MedicoDTO>>.Success(medicosDTO);
    }

    public async Task<Result<IEnumerable<MedicoDTO>>> GetAllMedicos()
    {
        var allMedicos = await _uow.Medicos.GetAllMedicos();
        if(allMedicos == null)
            return Result<IEnumerable<MedicoDTO>>.Failure(MedicosErrosResult.MedicosNaoEncontrados());
        
        var allMedicosDTO = _mapper.Map<IEnumerable<MedicoDTO>>(allMedicos);
        
        return Result<IEnumerable<MedicoDTO>>.Success(allMedicosDTO);
    }

    public async Task<Result<IEnumerable<MedicoDTO>>> GetMedicosByEspecialidade(Especialidades especialidade)
    {
        var medicosPorEspecialidade = await _uow.Medicos.GetMedicosByEspecialidade(especialidade);
        if(medicosPorEspecialidade == null)
            return Result<IEnumerable<MedicoDTO>>.Failure(MedicosErrosResult.NenhumMedicoParaEssaEspecialidade());
        
        var medicosPorEspecialidadeDTO = _mapper.Map<IEnumerable<MedicoDTO>>(medicosPorEspecialidade);
        return Result<IEnumerable<MedicoDTO>>.Success(medicosPorEspecialidadeDTO);
    }

    public async Task<Result<bool>> ExistsMedicoById(int id)
    {
        var exists = await _uow.Medicos.existsMedicoById(id);
        return Result<bool>.Success(exists);
    }

    public async Task<Result> SoftDeleteMedico(int id)
    {
        var medico = await _uow.Medicos.GetMedicoById(id);
        if (medico == null)
            return Result.Failure(MedicosErrosResult.MedicoNaoEncontrado());

        var result = medico.SoftDelete();
        if (!result)
        {
            return Result.Failure(MedicosErrosResult.MedicoJaInativo());
        }
        await _uow.CommitAsync();
        return Result.Success();
    }

    public async Task<Result<MedicoDTO>> GetMedicoByCRM(string crmNumber)
    {
        var medico = await _uow.Medicos.GetMedicoByCRM(crmNumber);
        if(medico == null)
            return Result<MedicoDTO>.Failure(MedicosErrosResult.CrmNaoLocalizado());
        var medicoDTO = _mapper.Map<MedicoDTO>(medico);
        return Result<MedicoDTO>.Success(medicoDTO);
    }
}