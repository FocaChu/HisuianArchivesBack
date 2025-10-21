using FluentValidation;
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
        /// Registers MediatR and scans the executing assembly for handlers, commands, queries and event handlers.
        /// </summary>
        /// <param name="services">The service collection to add the MediatR registrations to.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
        /// <remarks>
        /// This method calls <c>services.AddMediatR</c> with the currently executing assembly
        /// to automatically discover and register all MediatR handlers defined in this project.
        /// </remarks>
        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }

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
        /// - FluentValidation validators from the executing assembly
        /// </remarks>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        /// <summary>
        /// Registers all application-level services, mappings and orchestrators by invoking
        /// the specialized registration extension methods.
        /// </summary>
        /// <param name="services">The service collection to register application services into.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
        /// <remarks>
        /// This is a convenience method that aggregates <see cref="AddMediatR"/>,
        /// <see cref="AddApplicationMapping"/> and <see cref="AddServices"/> to ensure
        /// all required application dependencies are registered in one call.
        /// </remarks>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR();
            services.AddApplicationMapping();
            services.AddServices();

            return services;
        }
    }
}
