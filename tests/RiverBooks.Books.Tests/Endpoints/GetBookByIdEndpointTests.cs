using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using RiverBooks.Books.Endpoints.GetById;

namespace RiverBooks.Books.Tests.Endpoints;

public class GetBookByIdEndpointTests(Fixture fixture) : TestBase<Fixture>
{
  private readonly Fixture _fixture = fixture;

  [Theory]
  [InlineData("98999DE8-FBEF-4918-ADFE-DDA0F8B702D9", "The Fellowship of the Ring")]
  [InlineData("EB9DCAD0-6986-43FF-9A25-11FC79A31448", "The Two Towers")]
  [InlineData("F737D750-4634-492C-BE87-253896EDAF40", "The Return of the King")]
  public async Task ReturnsExpectedBookGivenIdAsync(string validId, string expectedTitle)
  {
    var id = Guid.Parse(validId);
    var request = new GetBookByIdRequest(id);
    var testResult = await _fixture.Client.GETAsync<GetBookByIdEndpoint, GetBookByIdRequest, BookDto>(request);

    testResult.Response.EnsureSuccessStatusCode();
    testResult.Result.Title.Should().Be(expectedTitle);
  }
}
