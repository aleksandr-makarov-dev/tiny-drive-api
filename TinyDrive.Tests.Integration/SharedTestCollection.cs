using Xunit;

namespace TinyDrive.Tests.Integration;

[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<CustomWebApplicationFactory>;
