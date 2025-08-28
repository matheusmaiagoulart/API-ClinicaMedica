using Microsoft.AspNetCore.Mvc;

namespace API_ClinicaMedica.Application.Results;


    public record Error(string Id, string mensagem, int StatusCode);

    public static class Errors
    {
        public static Error NotFound(string message) => new ("NotFound", message, StatusCodes.Status404NotFound);
        public static Error BadRequest { get; } = new("BadRequest",  "Erro ao realizar a requisição!", StatusCodes.Status400BadRequest);
        public static Error InternalServerError { get; } = new("InternalServerError",  "Erro interno no servidor!", StatusCodes.Status500InternalServerError);
        
    }
