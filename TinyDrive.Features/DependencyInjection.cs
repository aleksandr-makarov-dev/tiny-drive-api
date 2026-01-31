using Carter;
using FluentValidation;
using TinyDrive.Features.Common.Behaviours;

namespace TinyDrive.Features;

internal static class DependencyInjection
{
	public static void AddFeaturesServices(this IServiceCollection services)
	{
		services.AddCarter();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		services.AddMediatR(configuration =>
		{
			configuration.AddOpenBehavior(typeof(ValidationPipelineBehaviour<,>));

			configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
		});


		services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
	}
}
