using Ardalis.GuardClauses;

namespace RiverBooks.Users;

public class CartItem
{
  public Guid Id { get; private set; } = Guid.NewGuid();

  public Guid BookId { get; private set; }

  public string Description { get; private set; } = string.Empty;

  public int Quantity { get; private set; }

  public decimal UnitPrice { get; private set; }

  public CartItem(Guid bookId, string description, int quantity, decimal unitPrice)
  {
    BookId = Guard.Against.Default(bookId);
    Description = Guard.Against.NullOrEmpty(description);
    Quantity = Guard.Against.Negative(quantity);
    UnitPrice = Guard.Against.Negative(unitPrice);
  }

  public CartItem()
  {
    // EF
  }

  public void UpdateQuantity(int itemQuantity)
  {
    Quantity = itemQuantity;
  }
}