using FluentValidation;
using HisuianArchives.Application.Orchestrators;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HisuianArchives.Application
{
    /// <summary>
    /// Provides extension methods to register application-level services, orchestrators and mapping profiles
    /// into an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers AutoMapper and scans the executing assembly for AutoMapper profiles.
        /// </summary>
        /// <param name="services">The service collection to add the AutoMapper registrations to.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
        /// <remarks>
        /// This method calls <c>services.AddAutoMapper</c> with the currently executing assembly
        /// to automatically discover and register mapping profiles defined in this project.
        /// </remarks>
        public static IServiceCollection AddApplicationMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

            return services;
        }

        /// <summary>
        /// Registers concrete implementations of application services into the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to register the services into.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
        /// <remarks>
        /// Services registered:
        /// - <see cref="IUserService"/> -> <see cref="UserService"/>
        /// - <see cref="IAuthService"/> -> <see cref="AuthService"/>
        /// - <see cref="IUserOnboardingOrchestrator"/> -> <see cref="UserOnboardingOrchestrator"/> (if orchestrators are considered services here)
        /// </remarks>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserOnboardingOrchestrator, UserOnboardingOrchestrator>();

            return services;
        }

        /// <summary>
        /// Registers orchestrator classes needed by the application into the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to register orchestrators into.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
        /// <remarks>
        /// Orchestrators encapsulate higher-level flows and coordinate multiple services.
        /// Register each orchestrator here to keep registrations organized and discoverable.
        /// </remarks>
        public static IServiceCollection AddApplicationOrchestrators(this IServiceCollection services)
        {
            services.AddScoped<IUserOnboardingOrchestrator, UserOnboardingOrchestrator>();

            return services;
        }

        /// <summary>
        /// Registers all application-level services, mappings and orchestrators by invoking
        /// the specialized registration extension methods.
        /// </summary>
        /// <param name="services">The service collection to register application services into.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
        /// <remarks>
        /// This is a convenience method that aggregates <see cref="AddApplicationMapping"/>,
        /// <see cref="AddServices"/> and <see cref="AddApplicationOrchestrators"/> to ensure
        /// all required application dependencies are registered in one call.
        /// </remarks>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddApplicationMapping();
            services.AddServices();
            services.AddApplicationOrchestrators();

            return services;
        }
    }
}
