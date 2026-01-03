using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Time;
using TinyDrive.Infrastructure.Data;
using TinyDrive.Infrastructure.Data.Interceptors;
using TinyDrive.Infrastructure.Storage;
using TinyDrive.Infrastructure.Time;

namespace TinyDrive.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        services.AddSingleton<InlineDomainEventInterceptor>();

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("NpgSqlConnection"), npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .AddInterceptors(provider.GetRequiredService<InlineDomainEventInterceptor>())
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IApplicationDbContext>((provider) => provider.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<IAmazonS3>(_ =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = configuration["ObjectStorage:Endpoint"],
                ForcePathStyle = true
            };

            return new AmazonS3Client(configuration["ObjectStorage:AccessKey"],
                configuration["ObjectStorage:SecretKey"], config);
        });

        services.AddScoped<IObjectStorage, S3ObjectStorage>();

        return services;
    }
}
