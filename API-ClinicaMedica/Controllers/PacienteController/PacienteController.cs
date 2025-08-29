using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Application.Services.PacienteService.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_ClinicaMedica.Controllers.PacienteController;

[ApiController]
[Route("api/[controller]")]
public class PacienteController : ControllerBase
{
    private readonly IPacienteService _pacienteService;
    private readonly ILogger<PacienteController> _logger;
    private readonly IValidator<CreatePacienteDTO> _pacienteDTOValidator;
    private readonly IValidator<UpdatePacienteDTO> _pacienteUpdateDTOValidator;
    
    public PacienteController
    (
        IPacienteService pacienteService, 
        IValidator<CreatePacienteDTO> pacienteDTOValidator,
        IValidator<UpdatePacienteDTO> pacienteUpdateDTOValidator,
        ILogger<PacienteController> logger
    )
    {
        _pacienteService = pacienteService;
        _pacienteDTOValidator = pacienteDTOValidator;
        _pacienteUpdateDTOValidator = pacienteUpdateDTOValidator;
        _logger = logger;
    }

    [HttpPost]
    [Route("CreatePaciente")]
    public async Task<IActionResult> CreatePaciente([FromBody] CreatePacienteDTO dto)
    {
        try
        {
            var validacaoDTO = await _pacienteDTOValidator.ValidateAsync(dto);
            if (!validacaoDTO.IsValid)
            {
                var erros = validacaoDTO.Errors.Select(v => v.ErrorMessage).ToList();
                return BadRequest(erros);
            }
            var result = await _pacienteService.CreatePaciente(dto);
            if (result.IsFailure)
            {
                var error = result.Error;
                return StatusCode(error.StatusCode, error.mensagem);
            }
            
            return CreatedAtAction(nameof(CreatePaciente), result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpGet]
    [Route("GetPacienteById/{id}")]
    public async Task<IActionResult> GetPaciente(int id)
    {
        try
        {
            var result = await _pacienteService.GetPacienteById(id);
            if (result.IsFailure)
            {
                var error = result.Error;
                return StatusCode(error.StatusCode, error.mensagem);
            }
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
    [HttpGet]
    [Route("GetAllPacientes")]
    public async Task<IActionResult> GetAllPacientes()
    {
        try
        {
            var result = await _pacienteService.GetAllPacientes();
            if (result.IsFailure)
            {
                var error = result.Error;
                return StatusCode(error.StatusCode, error.mensagem);
            }
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpPut]
    [Route("UpdatePaciente")]
    public async Task<IActionResult> UpdatePaciente([FromBody] UpdatePacienteDTO dto)
    {
        try
        {
            var validationResult = await _pacienteUpdateDTOValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var erros = validationResult.Errors.Select(e => e.ErrorCode).ToList();
                return BadRequest(erros);
            }

            var resultUpdate = await _pacienteService.UpdatePaciente(dto.IdPaciente, dto);
            if (resultUpdate.IsFailure)
            {
                return StatusCode(resultUpdate.Error.StatusCode, resultUpdate.Error.mensagem);
            }
            return Ok(resultUpdate.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
        
        
    }
    
}