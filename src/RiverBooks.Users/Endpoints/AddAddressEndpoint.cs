using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User.AddAddress;

namespace RiverBooks.Users.Endpoints;

public class AddAddressEndpoint(IMediator mediator) : Endpoint<AddAddressRequest>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post("/users/addresses");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(AddAddressRequest req, CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var command = new AddAddressToUserCommand(emailAddress!,
      req.Street1,
      req.Street2,
      req.City,
      req.State,
      req.PostalCode,
      req.Country);

    var result = await _mediator.Send(command, ct);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
    }
    else
    {
      await SendOkAsync(ct);
    }
  }
}

public record AddAddressRequest(
  string Street1,
  string Street2,
  string City,
  string State,
  string PostalCode,
  string Country);
