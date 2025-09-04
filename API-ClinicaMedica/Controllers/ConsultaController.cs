using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.Interfaces;
using API_ClinicaMedica.Domain.Enums;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API_ClinicaMedica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultaController : ControllerBase
{
    private readonly IConsultaService _consultaService;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateConsultaDTO> _validationCreateConsulta;
    private readonly ILogger<MedicoController> _logger;

    public ConsultaController(IConsultaService consultaService, IMapper mapper,
        IValidator<CreateConsultaDTO> validationCreateConsulta, ILogger<MedicoController> logger)
    {
        _consultaService = consultaService;
        _mapper = mapper;
        _validationCreateConsulta = validationCreateConsulta;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AgendarConsulta(CreateConsultaDTO dto)
    {
        try
        {
            var result = await _validationCreateConsulta.ValidateAsync(dto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }

            var consultaResult = await _consultaService.AgendarConsulta(dto);
            if (consultaResult.IsFailure)
                return BadRequest(consultaResult.Error);

            var consultaDTO = _mapper.Map<ConsultaDTO>(consultaResult.Value);
            return CreatedAtAction(nameof(AgendarConsulta), consultaDTO);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpGet("GetConsultasByPaciente/{idPaciente}")]
    public async Task<IActionResult> GetConsultasByIdPaciente(int idPaciente)
    {
        try
        {
            var consultaResult = await _consultaService.GetConsultasByPacienteId(idPaciente);
            if (consultaResult.IsFailure)
                return NotFound(consultaResult.Error);

            return Ok(consultaResult.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
    
    [HttpGet("PesquisarPorPeriodo")]
    public async Task<IActionResult> GetConsultasByDateRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            var consultaResult = await _consultaService.GetConsultasByDateRange(startDate, endDate);
            if (consultaResult.IsFailure)
                return NotFound(consultaResult.Error);

            return Ok(consultaResult.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
    
    [HttpGet("PesquisarPorPeriodoPorMedico/{idMedico}")]
    public async Task<IActionResult> GetConsultasByDateRange(int idMedico, DateTime startDate, DateTime endDate)
    {
        try
        {
            var consultaResult = await _consultaService.GetMedicoConsultasByDateRange(idMedico, startDate, endDate);
            if (consultaResult.IsFailure)
                return NotFound(consultaResult.Error);

            return Ok(consultaResult.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
    
    
    [HttpPost("CancelarConsulta")]
    public async Task<IActionResult> CancelarConsulta(Guid id, MotivosCancelamentoConsulta motivo)
    {
        try
        {
            var consultaResult = await _consultaService.CancelarConsulta(id, motivo);
            if (consultaResult.IsFailure)
                return NotFound(consultaResult.Error);

            return Ok(consultaResult);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
    
    [HttpGet("GetConsultasByMedico/{idMedico}")]
    public async Task<IActionResult> GetConsultasByMedicoId(int idMedico)
    {
        try
        {
            var consultaResult = await _consultaService.GetConsultasByMedicoId(idMedico);
            if (consultaResult.IsFailure)
                return NotFound(consultaResult.Error);

            return Ok(consultaResult.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
    
    
}