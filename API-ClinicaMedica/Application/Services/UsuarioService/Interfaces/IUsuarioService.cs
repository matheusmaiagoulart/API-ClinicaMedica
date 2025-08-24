using API_ClinicaMedica.Application.DTOs.UpdateUsuarioDTOs;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Domain.Entities;

namespace API_ClinicaMedica.Application.Services.UsuarioService.Interfaces;

public interface IUsuarioService
{
    Task<Usuario> CreateUser(CreateUsuarioDTO dto);
    Task<UsuarioDTO> GetUserById(int id);
    Task<IEnumerable<UsuarioDTO>> GetAllUsers();
    
    //Task<bool> DeleteUser(int id);
    Task<UsuarioDTO> UpdateUser(int id, UpdateUsuarioDTO dto);
    
}