using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Minio;
using Testcontainers.PostgreSql;
using Xunit;

namespace TinyDrive.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private const string MinioUsername = "minioatest";
	private const string MinioPassword = "miniotest";
	private const string MinioBucket = "tiny_drive";

	private const string PostgresUsername = "postgres";
	private const string PostgresPassword = "postgres";
	private const string PostgresDatabase = "tiny_drive";

	private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:latest")
		.WithDatabase(PostgresDatabase)
		.WithUsername(PostgresUsername)
		.WithPassword(PostgresPassword)
		.Build();

	private readonly MinioContainer _minioContainer = new MinioBuilder("minio/minio:latest")
		.WithUsername(MinioUsername)
		.WithPassword(MinioPassword)
		.Build();

	public async ValueTask InitializeAsync()
	{
		await _dbContainer.StartAsync();
		await _minioContainer.StartAsync();
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseSetting("ConnectionStrings:DefaultConnection", _dbContainer.GetConnectionString());

		builder.UseSetting("Minio:Endpoint", $"http://{_minioContainer.GetConnectionString()}");
		builder.UseSetting("Minio:AccessKey", MinioUsername);
		builder.UseSetting("Minio:SecretKey", MinioPassword);
		builder.UseSetting("Minio:Bucket", MinioBucket);
	}

	public new async Task DisposeAsync()
	{
		await _dbContainer.DisposeAsync();
		await _minioContainer.DisposeAsync();
	}
}
