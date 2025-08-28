using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Domain.Enums;
using FluentValidation;

namespace API_ClinicaMedica.Application.FluentValidation;

public class UsuarioDTOValidator : AbstractValidator<CreateUsuarioDTO>
{
    public UsuarioDTOValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress().NotEmpty().NotNull().WithMessage("O email é obrigatório e deve ser válido.");
        
        RuleFor(u => u.Senha)
            .NotEmpty().NotNull().MinimumLength(6).WithMessage("A senha é obrigatória e deve ter no mínimo 6 caracteres.");
        
        // Informações Básicas
        
            RuleFor(u => u.InformacoesBasicas.Nome)
                .NotNull().NotEmpty().WithMessage("O Nome é obrigatório!");
            
            RuleFor(u => u.InformacoesBasicas.Cpf)
                .NotNull().NotEmpty().Length(11).WithMessage("O CPF é obrigatório e deve ter 11 caracteres.");
            
            RuleFor(u => u.InformacoesBasicas.Rg)
                .NotEmpty().NotNull().Length(9).WithMessage("O RG é obrigatório e deve ter 9 caracteres.");
            
            RuleFor(u => u.InformacoesBasicas.DataNascimento)
                .NotEmpty().NotNull().LessThan(DateTime.Now).WithMessage("A Data de Nascimento é obrigatória e deve ser menor que a data atual.");
            
            RuleFor(u => u.InformacoesBasicas.Telefone)
                .NotEmpty().NotNull().Length(10, 11).WithMessage("O Telefone é obrigatório e deve ter entre 10 e 11 caracteres.");
            
        // Endereço
        
            RuleFor(u => u.Endereco.Logradouro)
                .NotEmpty().NotNull().WithMessage("O Logradouro é obrigatório!");
            
            RuleFor(u => u.Endereco.Numero)
                .NotEmpty().NotNull().WithMessage("O Número é obrigatório!");
            
            RuleFor(u => u.Endereco.Bairro)
                .NotEmpty().NotNull().WithMessage("O Bairro é obrigatório!");
            
            RuleFor(u => u.Endereco.Cidade)
                .NotEmpty().NotNull().WithMessage("A Cidade é obrigatória!");

            RuleFor(u => u.Endereco.Estado)
                .NotEmpty().NotNull().IsInEnum().WithMessage("O Estado é obrigatório e deve ser um valor válido.");
            
            RuleFor(u => u.Endereco.Cep)
                .NotEmpty().NotNull().Length(8).WithMessage("O CEP é obrigatório e deve ter 8 caracteres.");
                
            
    }
}