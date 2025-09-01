using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.Results.GenericsResults;
using API_ClinicaMedica.Domain.Entities;

namespace API_ClinicaMedica.Application.Interfaces;

public interface IUsuarioService
{
    Task<Result<Usuario>> CreateUser(CreateUsuarioDTO dto);
    Task<Result<UsuarioDTO>> GetUserById(int id);
    Task<Result<IEnumerable<UsuarioDTO>>> GetAllUsers();
    
    //Task<bool> DeleteUser(int id);
    Task<Result<UsuarioDTO>> UpdateUser(int id, UpdateUsuarioDTO dto);
    
}