using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts.Commands;
using RiverBooks.OrderProcessing.Contracts.Models;

namespace RiverBooks.Users.UseCases.Cart.Checkout;

internal class CheckoutCartCommandHandler(IApplicationUserRepository userRepository, IMediator mediator)
  : IRequestHandler<CheckoutCartCommand, Result<Guid>>
{
  private readonly IApplicationUserRepository _userRepository = userRepository;
  private readonly IMediator _mediator = mediator;

  public async Task<Result<Guid>> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);

    if (user is null)
      return Result.Unauthorized();

    var items = user.CartItems.Select(item =>
        new OrderItemDetails(
          item.BookId,
          item.Quantity,
          item.UnitPrice,
          item.Description))
      .ToArray();

    var createOrderCommand = new CreateOrderCommand(
      Guid.Parse(user.Id),
      request.ShippingAddressId,
      request.BillingAddressId,
      items);

    Result<OrderDetailsResponse> result = await _mediator.Send(createOrderCommand, cancellationToken);

    if (!result.IsSuccess)
    {
      // We also want to keep all errors as-is. So here we're just casting the result object.
      return result.Map(x => x.OrderId);
    }

    user.ClearCart();

    await _userRepository.SaveChangesAsync();

    return Result.Success(result.Value.OrderId);
  }
}
