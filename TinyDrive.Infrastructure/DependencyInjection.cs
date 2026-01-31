using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyDrive.Infrastructure.Data;

namespace TinyDrive.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");

		Guard.Against.NullOrEmpty(connectionString);

		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseNpgsql(connectionString: connectionString);
			options.UseSnakeCaseNamingConvention();
		});
	}
}
