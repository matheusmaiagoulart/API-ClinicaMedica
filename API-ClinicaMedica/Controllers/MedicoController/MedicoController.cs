using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Application.Services.MedicoService.Interfaces;
using API_ClinicaMedica.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API_ClinicaMedica.Controllers.MedicoController;

[ApiController]
[Route("api/[controller]")]
public class MedicoController : ControllerBase
{
    private readonly IMedicoService _medicoService;
    private readonly ILogger<MedicoController> _logger;
    private readonly IValidator<CreateMedicoDTO> _createValidator;

    public MedicoController(
        IMedicoService medicoService,
        IValidator<CreateMedicoDTO> createValidator,
        ILogger<MedicoController> logger)
    {
        _medicoService = medicoService;
        _createValidator = createValidator;
        _logger = logger;
    }

    [HttpPost]
    [Route("CreateMedico")]
    public async Task<IActionResult> CreateMedico([FromBody] CreateMedicoDTO dto)
    {
        try
        {
            var validationDTOResult = await _createValidator.ValidateAsync(dto);
            if (!validationDTOResult.IsValid)
            {
                var erros = validationDTOResult.Errors.Select(v => v.ErrorMessage).ToList();
                return BadRequest(erros);
            }

            var result = await _medicoService.CreateMedico(dto);
            if (result.IsFailure)
            {
                var error = result.Error;
                return StatusCode(error.StatusCode, error.mensagem);
            }

            return CreatedAtAction(nameof(CreateMedico), result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpGet]
    [Route("GetMedicoById/{id}")]
    public async Task<IActionResult> GetMedicoById(int id)
    {
        try
        {
            var result = await _medicoService.GetMedicoById(id);
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
    [Route("GetAllMedicos")]
    public async Task<IActionResult> GetAllMedicos()
    {
        try
        {
            var result = await _medicoService.GetAllMedicos();
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
    [Route("GetAllActiveMedicos")]
    public async Task<IActionResult> GetAllActiveMedicos()
    {
        try
        {
            var result = await _medicoService.GetAllActiveMedicos();
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
    [Route("GetMedicosByEspecialidade")]
    public async Task<IActionResult> GetMedicosByEspecialidade(Especialidades especialidade)
    {
        try
        {
            var result = await _medicoService.GetMedicosByEspecialidade(especialidade);
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
    [Route("GetMedicosByEspecialidadeAndActiveTrue/{especialidade}")]
    public async Task<IActionResult> GetMedicosByEspecialidadeAndActiveTrue(Especialidades especialidade)
    {
        try
        {
            var result = await _medicoService.GetMedicosByEspecialidadeAndActiveTrue(especialidade);
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
    [Route("GetMedicoByCRM/{crmNumber}")]
    public async Task<IActionResult> GetMedicoByCRM(string crmNumber)
    {
        try
        {
            var result = await _medicoService.GetMedicoByCRM(crmNumber);
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

    [HttpDelete]
    [Route("SoftDeleteMedico/{id}")]
    public async Task<IActionResult> SoftDeleteMedico(int id)
    {
        try
        {
            var result = await _medicoService.SoftDeleteMedico(id);
            if (result.IsFailure)
            {
                var error = result.Error;
                return StatusCode(error.StatusCode, error.mensagem);
            }
            return Ok("Médico desativado com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }
}