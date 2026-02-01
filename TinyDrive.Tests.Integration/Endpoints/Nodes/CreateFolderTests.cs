using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shouldly;
using TinyDrive.Features.Features.Nodes.CreateFolder;
using Xunit;

namespace TinyDrive.Tests.Integration.Endpoints.Nodes;

public sealed class CreateFolderTests(CustomWebApplicationFactory webFactory) : IntegrationTestBase(webFactory)
{
	public override async ValueTask InitializeAsync()
	{
		await base.InitializeAsync();

		var nodes = await DbContext.Nodes.IgnoreQueryFilters().ToListAsync();

		DbContext.Nodes.RemoveRange(nodes);

		await DbContext.SaveChangesAsync();
	}

	[Fact]
	public async Task CreateFolder_WhenRequestIsValid_ShouldReturnStatusOkAndId()
	{
		// Arrange
		var request = new CreateFolderRequest("TestFolder", null);

		// Act
		var httpResponse = await HttpClient.PostAsJsonAsync("api/nodes", request,
			cancellationToken: TestContext.Current.CancellationToken);

		var folderResponse =
			await httpResponse.Content.ReadFromJsonAsync<Guid>(
				cancellationToken: TestContext.Current.CancellationToken);

		// Assert
		httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

		folderResponse.ShouldNotBe(Guid.Empty);
	}

	[Fact]
	public async Task CreateFolder_WhenParentFolderDoesNotExist_ShouldReturnStatusBadRequest()
	{
		// Act
		var parentId = Guid.NewGuid();
		var request = new CreateFolderRequest("TestFolder", parentId);

		// Arrange
		var httpResponse = await HttpClient.PostAsJsonAsync("api/nodes", request,
			cancellationToken: TestContext.Current.CancellationToken);

		var problemDetailsResponse =
			await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(
				cancellationToken: TestContext.Current.CancellationToken);

		// Assert
		httpResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);

		problemDetailsResponse.ShouldNotBeNull();
		problemDetailsResponse.Title.ShouldBe("Requested resource(s) not found.");
		problemDetailsResponse.Extensions.ShouldContainKey("errors");
	}
}
