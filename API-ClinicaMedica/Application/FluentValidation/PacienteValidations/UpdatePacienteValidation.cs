using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using FluentValidation;

namespace API_ClinicaMedica.Application.FluentValidation.PacienteValidations;

public class UpdatePacienteValidation : AbstractValidator<UpdatePacienteDTO>
{
    public UpdatePacienteValidation()
    {
        
    RuleFor(m => m.IdPaciente)
        .GreaterThan(0)
        .NotNull()
        .WithMessage("O campo 'IdPaciente' deve ser maior que zero.");
    
    
    RuleFor(p => p.Pcd)
        .NotNull().WithMessage("O campo 'Pcd' é obrigatório.");

    RuleForEach(p => p.MedicamentosControlados)
        .ChildRules(mc =>
    {
        mc.RuleFor(m => m.Nome)
            .MaximumLength(40).WithMessage("O campo 'Nome' deve ter no máximo 100 caracteres.");
        mc.RuleFor(m => m.Dosagem)
            .MaximumLength(40).WithMessage("O campo 'Dosagem' deve ter no máximo 40 caracteres.");
        mc.RuleFor(m => m.Frequencia)
            .MaximumLength(40).WithMessage("O campo 'Frequencia' deve ter no máximo 40 caracteres.");
        mc.RuleFor(m => m.Observacoes)
            .MaximumLength(150).WithMessage("O campo 'Observacoes' deve ter no máximo 150 caracteres.");
    });
}
}