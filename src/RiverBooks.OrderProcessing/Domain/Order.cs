using System.ComponentModel.DataAnnotations.Schema;
using RiverBooks.OrderProcessing.Domain.Events;
using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Domain;

internal class Order : IHaveDomainEvents
{
  public Guid Id { get; private set; } = Guid.NewGuid();

  public Guid UserId { get; private set; }

  public Address ShippingAddress { get; set; } = default!;

  public Address BillingAddress { get; set; } = default!;

  private readonly List<OrderItem> _orderItems = [];

  public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

  public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;

  private void AddOrderItem(OrderItem item) => _orderItems.Add(item);
  
  private readonly List<DomainEventBase> _domainEvents = [];

  [NotMapped]
  public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  
  void IHaveDomainEvents.ClearDomainEvents() => _domainEvents.Clear();

  /// <summary>
  /// A factory to create instances of the <see cref="Order"/> class which also raises the corresponding domain events.
  /// </summary>
  /// <remarks>It's a nested class because this way it can access private setters of the <see cref="Order"/> class.</remarks>
  internal class Factory
  {
    public static Order Create(Guid userId, Address shippingAddress, Address billingAddress,
      IReadOnlyCollection<OrderItem> orderItems)
    {
      var order = new Order();
      order.UserId = userId;
      order.ShippingAddress = shippingAddress;
      order.BillingAddress = billingAddress;

      foreach (var item in orderItems)
      {
        order.AddOrderItem(item);
      }

      var createdEvent = new OrderCreatedEvent(order);
      order.RegisterDomainEvent(createdEvent);

      return order;
    }
  }
}
