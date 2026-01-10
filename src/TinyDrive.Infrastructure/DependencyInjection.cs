using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Application.Abstract.Data.Repositories;
using TinyDrive.Infrastructure.Data;
using TinyDrive.Infrastructure.Data.Interceptors;
using TinyDrive.Infrastructure.Repositories;
using TinyDrive.Infrastructure.Time;
using TinyDrive.SharedKernel;

namespace TinyDrive.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddDatabase(configuration);

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<InlineDomainEventInterceptor>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("NpgSqlConnection"), npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .AddInterceptors(provider.GetRequiredService<InlineDomainEventInterceptor>())
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork>((provider) => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<INodeRepository, NodeRepository>();
    }
}
