using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Infrastructure.Data;
using TinyDrive.Infrastructure.Data.Interceptors;

namespace TinyDrive.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<InlineDomainEventInterceptor>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("NpgSqlConnection"), npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .AddInterceptors(provider.GetRequiredService<InlineDomainEventInterceptor>())
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IApplicationDbContext>((provider) => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
