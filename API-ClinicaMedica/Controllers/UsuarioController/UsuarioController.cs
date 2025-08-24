using API_ClinicaMedica.Application.DTOs.UpdateUsuarioDTOs;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Services.UsuarioService.Interfaces;
using API_ClinicaMedica.Infra.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API_ClinicaMedica.Controllers.UsuarioController;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioService _usuarioService;
    public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
    {
        _logger = logger;
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUsuarioDTO dto)
    {
        try
        {
            await _usuarioService.CreateUser(dto);
            return CreatedAtAction(nameof(CreateUser), dto);
        }
        catch (EmailException ex)
        {
            _logger.LogInformation(ex.Message);
            return BadRequest("Não foi possível cadastrar o usuário.");
        }
        catch (CpfException ex)
        {
            _logger.LogInformation(ex.Message);
            return BadRequest("Não foi possível cadastrar o usuário.");
        }
        catch (TelefoneException ex)
        {
            _logger.LogInformation(ex.Message);
            return BadRequest("Não foi possível cadastrar o usuário.");
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
                var user = await _usuarioService.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (UsuarioNaoEncontradoException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound("Usuário não encontrado.");
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
            var users = await _usuarioService.GetAllUsers();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500, "Ocorreu um erro ao processar sua requisição");
        }
    }

    [HttpPut("UpdateUsuario/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUsuarioDTO dto)
    {
        try
        {
            var updatedUser = await _usuarioService.UpdateUser(id, dto);
            return Ok(updatedUser);
        }
        catch (UsuarioNaoEncontradoException ex)
        {
            _logger.LogInformation(ex.Message);
            return NotFound("Usuário não encontrado.");
        } 
        catch (EmailException ex)
        {
            _logger.LogInformation(ex.Message);
            return BadRequest("Não foi possível cadastrar o usuário.");
        }
        catch (CpfException ex)
        {
            _logger.LogInformation(ex.Message);
            return BadRequest("Não foi possível cadastrar o usuário.");
        }
        catch (TelefoneException ex)
        {
            _logger.LogInformation(ex.Message);
            return BadRequest("Não foi possível cadastrar o usuário.");
        }       
                

        return StatusCode(500, "Ocorreu um erro ao processar sua requisição");

    }
}
