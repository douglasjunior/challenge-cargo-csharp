using CargoManagement.Api.Data;
using CargoManagement.Api.DTOs;
using CargoManagement.Api.Middleware;
using CargoManagement.Api.Repositories;
using CargoManagement.Api.Repositories.Interfaces;
using CargoManagement.Api.Services;
using CargoManagement.Api.Services.Interfaces;
using CargoManagement.Api.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// DbContext
builder.Services.AddDbContext<CargoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<ICargoQueryRepository, CargoQueryRepository>();
builder.Services.AddScoped<IManifestRepository, ManifestRepository>();

// Services
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<IManifestService, ManifestService>();

// Validators
builder.Services.AddScoped<IValidator<CreateCargoDto>, CargoValidator>();
builder.Services.AddScoped<IValidator<CreateManifestDto>, ManifestValidator>();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Cargo Management API",
        Version = "v1",
        Description = "API para gestão de cargas portuárias"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Global Exception Handler
app.UseMiddleware<GlobalExceptionHandler>();

// Swagger (sempre habilitado para o desafio)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cargo Management API v1");
});

app.UseCors();
app.MapControllers();

Log.Information("Cargo Management API iniciada");

app.Run();
