using HisuianArchives.Application.Interfaces;
using HisuianArchives.Application.Services;
using HisuianArchives.Domain.Entities;
using HisuianArchives.Domain.Repositories;
using HisuianArchives.Infrastructure.Repositories;
using HisuianArchives.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HisuianArchives.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrasructureServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static IServiceCollection AddInfrasructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddInfrasructure(this IServiceCollection services)
        {
            services.AddInfrasructureRepositories();
            services.AddInfrasructureServices();

            return services;
        }
    }
}
