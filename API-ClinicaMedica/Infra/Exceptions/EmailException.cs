namespace API_ClinicaMedica.Infra.Exceptions;

public class EmailException : Exception

{
    public EmailException(string email)
        : base($"O email '{email}' já está em uso por outro usuário.")
    {
    }
    
}