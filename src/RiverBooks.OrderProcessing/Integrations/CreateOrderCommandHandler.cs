using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Contracts.Commands;
using RiverBooks.OrderProcessing.Contracts.Models;

namespace RiverBooks.OrderProcessing.Integrations;

internal class CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger)
  : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
  private readonly IOrderRepository _orderRepository = orderRepository;
  private readonly ILogger<CreateOrderCommandHandler> _logger = logger;

  public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
  {
    var items = request.OrderItems.Select(oi => new OrderItem(
        oi.BookId, oi.Quantity, oi.UnitPrice, oi.Description))
      .ToArray();

    // TODO: Fetch real addresses.
    var shippingAddress = new Address("123 Main St", "", "Kent", "NY", "12345", "US");
    var billingAddress = shippingAddress;

    var newOrder = Order.Factory.Create(request.UserId, shippingAddress, billingAddress, items);

    await _orderRepository.AddAsync(newOrder);
    await _orderRepository.SaveChangesAsync();
    
    _logger.LogInformation("New Order Created. ID = {OrderId}", newOrder.Id);

    return new OrderDetailsResponse(newOrder.Id);
  }
}
