namespace API_ClinicaMedica.Infra.Exceptions;

public class TelefoneException : Exception
{
    public TelefoneException(string telefone)
        : base($"O telefone '{telefone}' já está em uso por outro usuário.")
    {
    }
    
}