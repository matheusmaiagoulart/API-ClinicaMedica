using API_ClinicaMedica.Application.Results.GenericsResults;

namespace API_ClinicaMedica.Application.Results.EntitiesResults;

public class ConsultaErrosResult
{
    public static Error AntecedenciaMinimaNecessaria() =>
        new ("AntecedenciaMinimaNecessaria", "A consulta precisa ser marcada com pelo menos 1h de antecedência.", StatusCodes.Status400BadRequest);
    
    public static Error DataInvalidaMaiorTresMeses() =>
        new ("DataInvalida", "A data da consulta tem que ser menor que três meses subsequentes.", StatusCodes.Status400BadRequest);
    
    public static Error DataInvalidaMenorQueAtual() =>
    new ("DataInvalidaMenorQueHoje", "A data da consulta não pode ser menor que a data atual.", StatusCodes.Status400BadRequest);
    
    public static Error DataForaDoHorarioDeAtendimento() =>
        new ("DataForaDoHorarioDeAtendimento", "A data da consulta precisa estar dentro do horário de atendimento (08:00 - 18:00).", StatusCodes.Status400BadRequest);
    
    public static Error NenhumMedicoDisponivel() => 
     new ("NenhumMedicoDisponivel","Nenhum médico disponível para a data e especialidade informadas.", StatusCodes.Status404NotFound);
    
    public static Error MedicoNaoDisponivelNaDataEscolhida() => 
     new ("MedicoNaoDisponivelNaDataEscolhida","O médico não está disponível na data escolhida.", StatusCodes.Status400BadRequest);
    
    public static Error HorarioNaoPermitido(TimeSpan proximaConsulta) =>
        new ("HorarioNaoPermitido", "Esse horário não é permitido! O Próximo horário é: " + proximaConsulta, StatusCodes.Status400BadRequest);
    
    public static Error PacienteSemConsultas() =>
        new ("PacienteSemConsultas", "Esse paciente não possui consultas.", StatusCodes.Status404NotFound);
    
    public static Error MedicoSemConsultas() =>
        new ("MedicoSemConsultas", "Esse Médico não possui consultas.", StatusCodes.Status404NotFound);
    public static Error SemConsultasParaEsteIntervalo() =>
        new ("SemConsultasParaEsteIntervalo", "Não há consultas para o intervalo procurado.", StatusCodes.Status404NotFound);
    
    public static Error ConsultaNaoEncontrada() =>
        new ("ConsultaNaoEncontrada", "Consulta não encontrada na base de dados.", StatusCodes.Status404NotFound);
    
    public static Error CancelamentoForaDoPrazo() =>
        new ("CancelamentoForaDoPrazo", "O cancelamento só pode ser feito com no mínimo 24h de antecedência.", StatusCodes.Status400BadRequest);
}