using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Domain.Enums;
using FluentValidation;

namespace API_ClinicaMedica.Application.FluentValidation.MedicoValidations;

public class CreateMedicoDTOValidation : AbstractValidator<CreateMedicoDTO>
{
    public CreateMedicoDTOValidation()
    {
        RuleFor(m => m.IdUsuario)
            .NotNull()
            .GreaterThan(0).WithMessage("IdUsuario deve ser maior que zero.");
        
        RuleFor(m => m.Especialidade)
            .Must(value => Enum.TryParse<Especialidades>(value, true, out _))
            .WithMessage("Especialidade inválida.");
        
        RuleFor(m => m.UfCrm)
            .NotNull().WithMessage("UF do CRM deve ser informada.")
            .NotEmpty().WithMessage("UF do CRM não pode ser vazia.")
            .Must(uf => Enum.TryParse<Estados>(uf, true, out _))
            .WithMessage("Estado inválido.");
        
        RuleFor(m => m.CrmNumber)
            .Matches("^[0-9]+$").WithMessage("O CRM do Médico deve conter apenas números.");
        
        RuleFor(m => m.Ativo)
            .NotNull().WithMessage("Ativo não pode ser nulo.");
            
    }
    
}