using API_ClinicaMedica.Domain.Entities;

namespace API_ClinicaMedica.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
   
    Task<Usuario> GetUserById(int id);
    Task<IEnumerable<Usuario>> GetAllUsers();

    Task<bool> isEmailAvailable(string login);
    
    Task<bool> isTelefoneAvailable(string telefone);
}