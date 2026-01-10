using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TinyDrive.Application.Abstract.Behaviors;

namespace TinyDrive.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });


        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true,
            lifetime: ServiceLifetime.Transient);

        return services;
    }
}
