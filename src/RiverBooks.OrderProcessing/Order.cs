namespace RiverBooks.OrderProcessing;

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
}
