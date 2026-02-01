using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using TinyDrive.Infrastructure.Data;
using Xunit;

namespace TinyDrive.Tests.Integration;

[Collection(nameof(SharedTestCollection))]
public abstract class IntegrationTestBase : IAsyncLifetime
{
	protected HttpClient HttpClient { get; }
	protected IServiceScope Scope { get; }

	protected ApplicationDbContext DbContext => Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	protected IAmazonS3 AmazonS3Client => Scope.ServiceProvider.GetRequiredService<IAmazonS3>();

	protected IntegrationTestBase(CustomWebApplicationFactory webFactory)
	{
		HttpClient = webFactory.CreateClient();
		Scope = webFactory.Services.CreateScope();
	}

	public virtual async ValueTask DisposeAsync()
	{
		Scope.Dispose();
		HttpClient.Dispose();

		await Task.CompletedTask;
	}

	public virtual async ValueTask InitializeAsync()
	{
		await Task.CompletedTask;
	}
}
