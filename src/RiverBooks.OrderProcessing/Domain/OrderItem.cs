using Ardalis.GuardClauses;

namespace RiverBooks.OrderProcessing.Domain;

internal class OrderItem
{
  public Guid Id { get; private set; } = Guid.NewGuid();

  public Guid BookId { get; private set; }

  public int Quantity { get; private set; }

  public decimal UnitPrice { get; private set; }

  public string Description { get; private set; } = string.Empty;

  public OrderItem(Guid bookId, int quantity, decimal unitPrice, string description)
  {
    BookId = Guard.Against.Default(bookId);
    Quantity = Guard.Against.Negative(quantity);
    UnitPrice = Guard.Against.Negative(unitPrice);
    Description = Guard.Against.NullOrEmpty(description);
  }
}
