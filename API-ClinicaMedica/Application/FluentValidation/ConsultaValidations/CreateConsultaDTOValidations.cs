using System.ComponentModel;
using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using FluentValidation;

namespace API_ClinicaMedica.Application.FluentValidation.ConsultaValidations;

public class CreateConsultaDTOValidations : AbstractValidator<CreateConsultaDTO>
{
    public CreateConsultaDTOValidations()
    {
        RuleFor(c => c.IdPaciente)
            .NotNull()
            .GreaterThan(0).WithMessage("IdPaciente deve ser maior que zero.");
        
        RuleFor(c => c.IdMedico)
            .NotNull()
            .GreaterThan(0).WithMessage("IdMedico deve ser maior que zero.");

        RuleFor(c => c.DataHoraConsulta)
            .NotNull().WithMessage("Data e hora da consulta devem ser informadas.");

    }
    
}