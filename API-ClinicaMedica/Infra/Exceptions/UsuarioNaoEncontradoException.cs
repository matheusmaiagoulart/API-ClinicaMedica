namespace API_ClinicaMedica.Infra.Exceptions;

public class UsuarioNaoEncontradoException : Exception
{
    public UsuarioNaoEncontradoException(string message) : base(message)
    {
        
    }
    
}