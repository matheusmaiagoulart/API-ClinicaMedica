using API_ClinicaMedica.Application.Results.GenericsResults;

namespace API_ClinicaMedica.Application.Results.EntitiesResults;

public class UsuarioErrosResults
{
    
    public static Error UsuarioNaoEncontrado() =>
    new ("UsuarioNaoEncontrado", "Usuário não encontrado na base de dados.", StatusCodes.Status404NotFound);
    
    public static Error UsuariosNaoEncontrado() =>
        new ("UsuarioNaoEncontrado", "Não foram encontrados Usuários na base de dados.", StatusCodes.Status404NotFound);
    public static Error EmailJaCadastrado(string email) =>
        new ("EmailJaCadastrado", $"O email '{email}' já está cadastrado na base de dados.", StatusCodes.Status400BadRequest);
    
    public static Error CpfJaCadastrado(string cpf) =>
        new ("CpfJaCadastrado", $"O CPF '{cpf}' já está cadastrado na base de dados.", StatusCodes.Status400BadRequest);
    
    public static Error TelefoneJaCadastrado(string telefone) =>
        new ("TelefoneJaCadastrado", $"O telefone '{telefone}' já está cadastrado na base de dados.", StatusCodes.Status400BadRequest);
}