using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using RiverBooks.Books.Endpoints.List;

namespace RiverBooks.Books.Tests.Endpoints;

public class ListBooksEndpointTests(Fixture fixture) : TestBase<Fixture>
{
  private readonly Fixture _fixture = fixture;
  
  [Fact]
  public async Task ReturnsThreeBooksAsync()
  {
    var testResult = await _fixture.Client.GETAsync<ListBooksEndpoint, ListBooksResponse>();

    testResult.Response.EnsureSuccessStatusCode();
    testResult.Result.Books.Count.Should().Be(3);
  }
}
