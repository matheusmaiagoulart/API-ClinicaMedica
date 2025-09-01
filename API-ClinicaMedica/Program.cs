using System.Text.Json.Serialization;
using API_ClinicaMedica.Application.DTOs.MedicoDTOs;
using API_ClinicaMedica.Application.DTOs.PacienteDTOs;
using API_ClinicaMedica.Application.DTOs.UsuarioDTOs;
using API_ClinicaMedica.Application.FluentValidation.MedicoValidations;
using API_ClinicaMedica.Application.FluentValidation.PacienteValidations;
using API_ClinicaMedica.Application.FluentValidation.UsuarioValidations;
using API_ClinicaMedica.Application.Profiles;
using API_ClinicaMedica.Application.Profiles.MedicoProfiles;
using API_ClinicaMedica.Application.Profiles.PacienteProfiles;
using API_ClinicaMedica.Application.Profiles.UsuarioProfiles;
using API_ClinicaMedica.Application.Services.MedicoService.Implementations;
using API_ClinicaMedica.Application.Services.MedicoService.Interfaces;
using API_ClinicaMedica.Application.Services.PacienteService.Implementations;
using API_ClinicaMedica.Application.Services.PacienteService.Interfaces;
using API_ClinicaMedica.Application.Services.UsuarioService.Implementations;
using API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Implementation;
using Microsoft.EntityFrameworkCore;
using API_ClinicaMedica.Application.Services.UsuarioService.Interfaces;
using API_ClinicaMedica.Application.Validations.UsuarioValidationInformacoesBasicas.Interface;
using API_ClinicaMedica.Infra.Repositories.Implementations;
using API_ClinicaMedica.Infra.Repositories.Interfaces;
using API_ClinicaMedica.Infra.Repositories.UnitOfWork;
using API_ClinicaMedica.Infra.Data.DbContext;
using API_ClinicaMedica.Middleware;
using FluentValidation;

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
builder.Services.AddAutoMapper(typeof(UsuarioProfile).Assembly);
builder.Services.AddAutoMapper(typeof(PacienteProfile).Assembly);
builder.Services.AddAutoMapper(typeof(MedicoProfile).Assembly);
builder.Services.AddScoped<IValidacaoInformacoesBasicas, ValidacaoEmail>();
builder.Services.AddScoped<IValidacaoInformacoesBasicas, ValidacaoCpf>();
builder.Services.AddScoped<IValidacaoInformacoesBasicas, ValidacaoTelefone>();


//Adição dos validadores FluentValidation
//Usuario Validations
builder.Services.AddScoped<IValidator<CreateUsuarioDTO>, UsuarioDTOValidator>();
//Paciente Validations
builder.Services.AddScoped<IValidator<CreatePacienteDTO>, CreatePacienteDTOValidation>();
builder.Services.AddScoped<IValidator<UpdatePacienteDTO>, UpdatePacienteValidation>();
//Medico Validations
builder.Services.AddScoped<IValidator<CreateMedicoDTO>, CreateMedicoDTOValidation>();


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
