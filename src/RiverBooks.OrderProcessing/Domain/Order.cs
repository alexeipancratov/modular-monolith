namespace RiverBooks.OrderProcessing.Domain;

internal class Order
{
  public Guid Id { get; private set; } = Guid.NewGuid();

  public Guid UserId { get; private set; }

  public Address ShippingAddress { get; set; } = default!;

  public Address BillingAddress { get; set; } = default!;

  private readonly List<OrderItem> _orderItems = [];

  public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

  public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;

  private void AddOrderItem(OrderItem item) => _orderItems.Add(item);

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

      return order;
    }
  }
}
