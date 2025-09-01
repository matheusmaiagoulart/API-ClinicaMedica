using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Application.Results.PacientesResults;
using API_ClinicaMedica.Application.Results.UsuariosResults;
using API_ClinicaMedica.Application.Services.PacienteService.Interfaces;
using API_ClinicaMedica.Domain.Entities;
using API_ClinicaMedica.Infra.Repositories.UnitOfWork;
using AutoMapper;

namespace API_ClinicaMedica.Application.Services.PacienteService.Implementations;

public class PacienteService : IPacienteService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public PacienteService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
    
    
    public async Task<Result<Paciente>> CreatePaciente(CreatePacienteDTO dto)
    {
        //Validação da existência do usuário
        var usuario = await _uow.Usuarios.existsById(dto.IdUsuario);
        if(usuario == false)
            return Result<Paciente>.Failure(UsuariosErrosResults.UsuarioNaoEncontrado());
        
        //Validação se o usuário já está vinculado a um paciente, procurando o IdUsuario na tabela Pacientes
        var jaVinculado = await _uow.Pacientes.existsById(dto.IdUsuario);
        if(jaVinculado)
            return Result<Paciente>.Failure(PacientesErrorsResult.IdJaVinculadoAUsuario());
        
        //Criação do paciente e vinculação com o Usuario passado
        var newPaciente = _mapper.Map<Paciente>(dto);
        newPaciente.setPacienteId(dto.IdUsuario);
        newPaciente.setAtivoTrue();
        await _uow.Pacientes.AddAsync(newPaciente);
        await _uow.CommitAsync();
        return Result<Paciente>.Success(newPaciente);
        
    }

    public async Task<Result<PacienteDTO>> GetPacienteById(int id)
    {
        var paciente = await _uow.Pacientes.GetPacienteById(id);
        if (paciente == null)
        {
            return Result<PacienteDTO>.Failure(PacientesErrorsResult.PacienteNaoEncontrado());
        }
        var pacienteDto = _mapper.Map<PacienteDTO>(paciente);
        return Result<PacienteDTO>.Success(pacienteDto);
    }

    public async Task<Result<IEnumerable<PacienteDTO>>> GetAllPacientes()
    {
       var allPacientes = await _uow.Pacientes.GetAllPacientes();
       
       if (allPacientes == null)
           return Result<IEnumerable<PacienteDTO>>.Failure(PacientesErrorsResult.PacientesNaoEncontrados());
       
       var pacientesDTO = _mapper.Map<IEnumerable<PacienteDTO>>(allPacientes);
         
        return Result<IEnumerable<PacienteDTO>>.Success(pacientesDTO);
    }


    public async Task<Result> SoftDeletePaciente(int id)
    {
        var paciente = await _uow.Pacientes.GetPacienteById(id);
        if(paciente == null)
            return Result<Paciente>.Failure(PacientesErrorsResult.PacienteNaoEncontrado());
        
        paciente.Desativar();
        await _uow.CommitAsync();
        return Result.Success();
        
    }

    public async Task<Result<PacienteDTO>> UpdatePaciente(int id, UpdatePacienteDTO dto)
    {
        var paciente = await _uow.Pacientes.GetPacienteById(id);
        if(paciente == null)
            return Result<PacienteDTO>.Failure(PacientesErrorsResult.PacienteNaoEncontrado());
        
        var pcienteAtualizado = _mapper.Map(dto, paciente);
        
        await _uow.CommitAsync();
        var pacienteDto = _mapper.Map<PacienteDTO>(pcienteAtualizado);
        return Result<PacienteDTO>.Success(pacienteDto); 
    }
}