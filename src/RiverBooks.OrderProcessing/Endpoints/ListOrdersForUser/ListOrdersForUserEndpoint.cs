using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;

namespace RiverBooks.OrderProcessing.Endpoints.ListOrdersForUser;

public class ListOrdersForUserEndpoint(IMediator mediator) : EndpointWithoutRequest<ListOrdersForUserResponse>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Get("/orders");
    Claims("EmailAddress");
  }

  public async override Task HandleAsync(CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var query = new ListOrdersForUserQuery(emailAddress!);

    var result = await _mediator.Send(query, ct);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
    }
    else
    {
      var response = new ListOrdersForUserResponse { Orders = result.Value };
      await SendAsync(response, 200, ct);
    }
  }
}
