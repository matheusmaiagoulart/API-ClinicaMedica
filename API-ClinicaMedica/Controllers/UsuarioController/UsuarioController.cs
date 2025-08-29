using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results;
using API_ClinicaMedica.Application.Services.UsuarioService.Interfaces;
using API_ClinicaMedica.Infra.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_ClinicaMedica.Controllers.UsuarioController;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IValidator<CreateUsuarioDTO> _createValidator;
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioService _usuarioService;
    public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger, IValidator<CreateUsuarioDTO> createValidator)
    {
        _logger = logger;
        _usuarioService = usuarioService;
        _createValidator = createValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUsuarioDTO dto)
    {
        try
        {
            //Validação do DTO de entrada 
            var validationDTOResult = await _createValidator.ValidateAsync(dto);
            if (!validationDTOResult.IsValid)
            {
                var erros = validationDTOResult.Errors.Select(v => v.ErrorMessage).ToList();
                return BadRequest(erros);
            }
            
            //Chamada do serviço para criar o usuário
            var result = await _usuarioService.CreateUser(dto);
            if (result.IsFailure)
            {
                var error = result.Error;
                return StatusCode(error.StatusCode, error.mensagem);
            }
            
            return CreatedAtAction(nameof(CreateUser), result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpGet]
    [Route("GetUsuarioById/{id}")]
    public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _usuarioService.GetUserById(id);
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
    [Route("GetAllUsuarios")]
    public async Task<IActionResult> GetAllUsuarios()
    {
        try
        {
            var result = await _usuarioService.GetAllUsers();
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

    [Authorize]
    [HttpPut("UpdateUsuario/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUsuarioDTO dto)
    {
        try
        {
            var result = await _usuarioService.UpdateUser(id, dto);
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
}
