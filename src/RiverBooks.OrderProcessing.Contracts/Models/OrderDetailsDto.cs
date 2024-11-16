namespace RiverBooks.OrderProcessing.Contracts.Models;

/// <summary>
/// Basic details of the order.
/// TODO: include address info.
/// </summary>
public class OrderDetailsDto
{
  public Guid OrderId { get; set; }

  public Guid UserId { get; set; }

  public DateTimeOffset DateCreated { get; set; }

  public IReadOnlyCollection<OrderItemDetails> OrderItems { get; set; } = [];
}
