using System.Security.Claims;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.Cart.Checkout;

namespace RiverBooks.Users.CartEndpoints.Checkout;

internal class CheckoutEndpoint(IMediator mediator) : Endpoint<CheckoutRequest, CheckoutResponse>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post("/cart/checkout");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CheckoutRequest req, CancellationToken ct)
  {
    var emailAddress = User.FindFirstValue("EmailAddress");

    var command = new CheckoutCartCommand(emailAddress!, req.ShippingAddressId, req.BillingAddressId);

    var result = await _mediator.Send(command, ct);

    if (result.Status == ResultStatus.Unauthorized)
    {
      await SendUnauthorizedAsync(ct);
    }
    else
    {
      await SendOkAsync(new CheckoutResponse(result.Value), ct);
    }
  }
}

internal record CheckoutResponse(Guid NewOrderId);

internal record CheckoutRequest(Guid ShippingAddressId, Guid BillingAddressId);
