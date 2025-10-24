using CompositionRoot.DependencyInjection;
using HealthChecks.UI.Client;
using HisuianArchives.Api.Filters;
using HisuianArchives.Api.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHisuianArchivesServices(builder.Configuration);

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