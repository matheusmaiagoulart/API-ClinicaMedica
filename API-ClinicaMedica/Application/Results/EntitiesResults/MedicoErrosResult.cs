using API_ClinicaMedica.Application.Results.GenericsResults;

namespace API_ClinicaMedica.Application.Results.EntitiesResults;

public class MedicoErrosResult
{
    public static Error IdJaVinculadoAUsuario() =>
        new ("IdJaVinculadoAUsuario", "O Id do usuário já está vinculado a um Médico na base de dados.", StatusCodes.Status400BadRequest);
    
    public static Error MedicoNaoEncontrado() =>
        new ("MedicoNaoEncontrado", "O Id do Médico não foi encontrado na base de dados.", StatusCodes.Status404NotFound);
    
    public static Error MedicosNaoEncontrados() =>
        new ("MedicosNaoEncontrados", "Não foram encontrados Médicos na base de dados.", StatusCodes.Status400BadRequest);
    
    public static Error NenhumMedicoParaEssaEspecialidade() =>
        new Error("NenhumMedicoParaEssaEspecialidade", "Nenhum médico encontrado para essa especialidade.", StatusCodes.Status404NotFound);
    
    public static Error MedicosAtivosNaoEncontrados() =>
        new ("MedicosAtivosNaoEncontrados", "Não foram encontrados Médicos ativos na base de dados.", StatusCodes.Status404NotFound);
    
    public static Error MedicoNaoPossuiEspecialidade() => 
    new ("MedicoNaoPossuiEspecialidade", "O Médico não possui a especialidade informada.", StatusCodes.Status400BadRequest);
    
    public static Error MedicoJaInativo() =>
        new ("MedicoJaInativo", "O Médico já consta inativo e não pode ser desativado.", StatusCodes.Status400BadRequest);
    
    public static Error MedicoInativo() =>
        new ("MedicoInativo", "O Médico está inativo.", StatusCodes.Status400BadRequest);
    
    public static Error CrmNaoLocalizado() =>
        new ("CrmNaoLocalizado", "Não foi encontrado um Médico com este CRM na base.", StatusCodes.Status404NotFound);
    
    public static Error CrmJaExistenteNoDB() =>
        new ("CrmJaExistenteNoDB", "O CRM em questão já consta cadastrado.", StatusCodes.Status400BadRequest);

}