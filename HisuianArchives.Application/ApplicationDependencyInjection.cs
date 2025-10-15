using HisuianArchives.Application.Orchestrators;
using HisuianArchives.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HisuianArchives.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IUserOnboardingOrchestrator, UserOnboardingOrchestrator>();

            return services;
        }

        public static IServiceCollection AddApplicationOrchestrators(this IServiceCollection services)
        {
            services.AddScoped<IUserOnboardingOrchestrator, UserOnboardingOrchestrator>();

            return services;
        }

        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
        {
            services.AddApplicationMapping();
            services.AddApplicationServices();
            services.AddApplicationOrchestrators();

            return services;
        }
    }
}
