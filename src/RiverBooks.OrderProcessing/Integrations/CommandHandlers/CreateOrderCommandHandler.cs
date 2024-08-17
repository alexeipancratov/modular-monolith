using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Contracts.Commands;
using RiverBooks.OrderProcessing.Contracts.Models;
using RiverBooks.OrderProcessing.MaterializedViews;

namespace RiverBooks.OrderProcessing.Integrations.CommandHandlers;

internal class CreateOrderCommandHandler(
  IOrderRepository orderRepository,
  ILogger<CreateOrderCommandHandler> logger,
  IOrderAddressCache addressCache)
  : IRequestHandler<CreateOrderCommand, Result<OrderDetailsResponse>>
{
  private readonly IOrderRepository _orderRepository = orderRepository;
  private readonly ILogger<CreateOrderCommandHandler> _logger = logger;
  private readonly IOrderAddressCache _addressCache = addressCache;

  public async Task<Result<OrderDetailsResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
  {
    var items = request.OrderItems.Select(oi => new OrderItem(
        oi.BookId, oi.Quantity, oi.UnitPrice, oi.Description))
      .ToArray();
    
    var shippingAddress = await _addressCache.GetByIdAsync(request.ShippingAddressId);
    var billingAddress = await _addressCache.GetByIdAsync(request.BillingAddressId);

    var newOrder = Order.Factory.Create(request.UserId,
      shippingAddress.Value.Address,
      billingAddress.Value.Address,
      items);

    await _orderRepository.AddAsync(newOrder);
    await _orderRepository.SaveChangesAsync();
    
    _logger.LogInformation("New Order Created. ID = {OrderId}", newOrder.Id);

    return new OrderDetailsResponse(newOrder.Id);
  }
}
