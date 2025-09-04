using System.Text.Json.Serialization;
using API_ClinicaMedica.Application.BusinessValidations.MarcarConsultaValidations;
using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Implementation;
using API_ClinicaMedica.Application.BusinessValidations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Application.Constants;
using API_ClinicaMedica.Application.DTOs.ConsultaDTOs;
using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.FluentValidation.ConsultaValidations;
using API_ClinicaMedica.Application.FluentValidation.MedicoValidations;
using API_ClinicaMedica.Application.FluentValidation.PacienteValidations;
using API_ClinicaMedica.Application.FluentValidation.UsuarioValidations;
using API_ClinicaMedica.Application.Interfaces;
using API_ClinicaMedica.Application.Profiles.EntitiesProfiles;
using API_ClinicaMedica.Application.Services;
using Microsoft.EntityFrameworkCore;
using API_ClinicaMedica.Infra.Data.DbContext;
using API_ClinicaMedica.Infra.Interfaces;
using API_ClinicaMedica.Infra.Repositories;
using API_ClinicaMedica.Middleware;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);


//Trasnforma o numero do enum em string no json e reconhece na hora de desserializar
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddAutoMapper(typeof(UsuarioProfile).Assembly);
builder.Services.AddAutoMapper(typeof(PacienteProfile).Assembly);
builder.Services.AddAutoMapper(typeof(MedicoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ConsultaProfile).Assembly);
builder.Services.AddScoped<IValidacaoInformacoesBasicas, ValidacaoEmail>();
builder.Services.AddScoped<IValidacaoInformacoesBasicas, ValidacaoCpf>();
builder.Services.AddScoped<IValidacaoInformacoesBasicas, ValidacaoTelefone>();
//Validações de negócio para o usuário
builder.Services.AddScoped<IMarcarConsultaValidator, MedicoDisponibilidadeValidator>();
builder.Services.AddScoped<IMarcarConsultaValidator, UsuarioValidoValidator>();
builder.Services.AddScoped<IMarcarConsultaValidator, MedicoValidoValidator>();
builder.Services.AddScoped<IMarcarConsultaValidator, HorarioFuncionamentoClinicaValidator>();
builder.Services.AddScoped<IMarcarConsultaValidator, DataValidator>();
builder.Services.AddScoped<ValidarHoraConsulta>();


//Adição dos validadores FluentValidation
//Usuario Validations
builder.Services.AddScoped<IValidator<CreateUsuarioDTO>, UsuarioDTOValidator>();
//Paciente Validations
builder.Services.AddScoped<IValidator<CreatePacienteDTO>, CreatePacienteDTOValidation>();
builder.Services.AddScoped<IValidator<UpdatePacienteDTO>, UpdatePacienteValidation>();
//Medico Validations
builder.Services.AddScoped<IValidator<CreateMedicoDTO>, CreateMedicoDTOValidation>();
//Consulta Validations
builder.Services.AddScoped<IValidator<CreateConsultaDTO>, CreateConsultaDTOValidations>();


//Conexão com o banco de dados
builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("ClinicaMedicaContext")));

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MiddlewareApplication.ErrorHandleMiddleware>(); // Adiciona o middleware
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
