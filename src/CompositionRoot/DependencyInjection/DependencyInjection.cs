using HisuianArchives.Application;
using HisuianArchives.Infrastructure;

namespace CompositionRoot.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddHisuianArchivesServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);

        return services;
    }
}
