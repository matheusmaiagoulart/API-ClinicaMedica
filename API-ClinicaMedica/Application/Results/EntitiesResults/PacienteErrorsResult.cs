using API_ClinicaMedica.Application.Results.GenericsResults;

namespace API_ClinicaMedica.Application.Results.EntitiesResults;

public class PacienteErrorsResult
{
    public static Error PacienteNaoEncontrado() =>
        new("PacienteNaoEncontrado", "Paciente não encontrado na base de dados.", StatusCodes.Status404NotFound);

    public static Error IdJaVinculadoAUsuario() => 
        new("IdJaVinculadoAUsuario", "O Id do usuário já está vinculado a um paciente na base de dados.", StatusCodes.Status400BadRequest);

    public static Error PacientesNaoEncontrados() =>
        new("PacientesNaoEncontrados", "Nenhum paciente encontrado na base de dados.", StatusCodes.Status404NotFound);
    
    public static Error PacienteInativo() =>
        new("PacienteInativo", "O paciente está inativo.", StatusCodes.Status404NotFound);
}