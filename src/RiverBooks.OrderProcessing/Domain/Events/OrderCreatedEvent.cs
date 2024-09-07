using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Domain.Events;

internal class OrderCreatedEvent(Order order) : DomainEventBase
{
  public Order Order { get; } = order;
}
