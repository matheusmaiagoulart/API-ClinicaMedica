namespace API_ClinicaMedica.Application.Results.PacientesResults;

public class PacientesErrorsResult
{
    public static Error PacienteNaoEncontrado() =>
        new ("PacienteNaoEncontrado", "Paciente não encontrado na base de dados.", StatusCodes.Status404NotFound);
    
    public static Error IdJaVinculadoAUsuario() =>
    new ("IdJaVinculadoAUsuario", "O Id do usuário já está vinculado a um paciente na base de dados.", StatusCodes.Status400BadRequest);
    
    public static Error PacientesNaoEncontrados() =>
        new ("PacientesNaoEncontrados", "Nenhum paciente encontrado na base de dados.", StatusCodes.Status404NotFound);
}