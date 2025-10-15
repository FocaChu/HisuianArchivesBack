using HealthChecks.UI.Client;
using HisuianArchives.Api.Middleware;
using HisuianArchives.Application;
using HisuianArchives.Application.Interfaces;
using HisuianArchives.Application.Orchestrators;
using HisuianArchives.Application.Services;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;
using HisuianArchives.Infrastructure;
using HisuianArchives.Infrastructure.Persistence;
using HisuianArchives.Infrastructure.Repositories;
using HisuianArchives.Infrastructure.Security;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS to allow requests from Angular frontend
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,policy =>
    {
        policy.WithOrigins
        (
            "http://localhost:4200",
            "https://hisuian-archives.vercel.app",
            "https://*.ngrok-free.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


// Configure Database
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Configure JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configure infrastructure 
builder.Services.AddInfrasructure();

// Register application 
builder.Services.AddApplicationDependencyInjection();


var app = builder.Build();


app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();