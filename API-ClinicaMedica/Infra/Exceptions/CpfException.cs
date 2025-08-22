namespace API_ClinicaMedica.Infra.Exceptions;

public class CpfException : Exception
{
    public CpfException(string cpf)
        : base($"O CPF '{cpf}' já está cadastrado.")
    {
    }
}