using HisuianArchives.Domain.Repositories;
using HisuianArchives.Infrastructure.Persistence.Data;
using HisuianArchives.Infrastructure.Persistence.Interceptors;
using HisuianArchives.Infrastructure.Repositories;
using HisuianArchives.Infrastructure.Security;
using HisuianArchives.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HisuianArchives.Infrastructure;

/// <summary>
/// Provides extension methods to register infrastructure services into the <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services (database, security, repositories) into the application's dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration used to read settings such as connection strings and JWT configuration.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddSecurity(configuration)
            .AddExternalServices()
            .AddRepositories();

        return services;
    }

    /// <summary>
    /// Configures the database-related services:
    /// - Registers the DB context with a scoped auditable entity interceptor.
    /// - Adds health checks for the MySQL database.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration used to read the connection string named "DefaultConnection".</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the "DefaultConnection" connection string is not configured.</exception>
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");

        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<HisuianArchivesDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                   .AddInterceptors(interceptor);
        });

        services.AddHealthChecks().AddMySql(connectionString, name: "MySQL Database");

        return services;
    }

    /// <summary>
    /// Configures security services including authentication and token generation.
    /// Registers identity, password and token services and configures JWT Bearer authentication based on configuration values.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration used to read JWT settings such as Key, Issuer and Audience.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the JWT signing key configuration ("Jwt:Key") is missing.</exception>
    private static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<IPasswordService, BcryptPasswordService>();
        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        return services;
    }

    private static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, BrevoEmailService>();

        return services;
    }

    /// <summary>
    /// Registers repository implementations used by the application domain.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}