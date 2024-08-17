using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Cart.ListItems;

namespace RiverBooks.Users.CartEndpoints.ListItems;

public class ListCartItemsEndpoint(IMediator mediator) : EndpointWithoutRequest<ListCartItemsResponse>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Get("/cart");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var query = new ListCartItemsQuery(emailAddress!);

    var result = await _mediator.Send(query, ct);
    
    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
    }
    else
    {
      var response = new ListCartItemsResponse { CartItems = result.Value };
      await SendAsync(response, 200, ct);
    }
  }
}
