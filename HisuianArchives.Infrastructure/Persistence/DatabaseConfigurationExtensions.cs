using HisuianArchives.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HisuianArchives.Infrastructure.Persistence;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("A connection string 'DefaultConnection' não está configurada.");
        }

        // Configurar Entity Framework e MySQL
        services.AddDbContext<HisuianArchivesDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // Registrar Health Checks para o banco
        services.AddHealthChecks()
            .AddMySql(connectionString, name: "MySQL Database");

        return services;
    }
}
