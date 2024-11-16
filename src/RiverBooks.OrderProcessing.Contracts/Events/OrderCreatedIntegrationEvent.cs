using MediatR;
using RiverBooks.OrderProcessing.Contracts.Models;

namespace RiverBooks.OrderProcessing.Contracts.Events;

public class OrderCreatedIntegrationEvent(OrderDetailsDto orderDetails) : INotification
{
  public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;
  
  public OrderDetailsDto OrderDetails { get; private set; } = orderDetails;
}
