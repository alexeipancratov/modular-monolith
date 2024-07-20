using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

public class ApplicationUser : IdentityUser
{
  public string FullName { get; set; } = string.Empty;

  private readonly List<CartItem> _cartItems = new();
  public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

  public void AddCardItem(CartItem item)
  {
    Guard.Against.Null(item);

    var existingBookItem = _cartItems.SingleOrDefault(i => i.BookId == item.BookId);
    if (existingBookItem != null)
    {
      existingBookItem.UpdateQuantity(existingBookItem.Quantity + item.Quantity);
      // TODO: What to do if other details of the item have been updated?
      return;
    }

    _cartItems.Add(item);
  }
}
