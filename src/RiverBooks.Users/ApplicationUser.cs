using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

public class ApplicationUser : IdentityUser
{
  public string FullName { get; set; } = string.Empty;

  private readonly List<CartItem> _cartItems = new();
  public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
  
  private readonly List<UserStreetAddress> _addresses = new();
  public IReadOnlyCollection<UserStreetAddress> Addresses => _addresses.AsReadOnly();

  public void AddCardItem(CartItem item)
  {
    Guard.Against.Null(item);

    var existingBookItem = _cartItems.SingleOrDefault(i => i.BookId == item.BookId);
    if (existingBookItem != null)
    {
      // TODO: The question still remains what happens if these details get updated outside of user adding one more item.
      existingBookItem.UpdateQuantity(existingBookItem.Quantity + item.Quantity);
      existingBookItem.UpdateDescription(existingBookItem.Description);
      existingBookItem.UpdatePrice(existingBookItem.UnitPrice);
      return;
    }

    _cartItems.Add(item);
  }

  public void ClearCart()
  {
    _cartItems.Clear();
  }

  public UserStreetAddress AddAddress(Address addressToAdd)
  {
    var userStreetAddress = new UserStreetAddress(Id, addressToAdd);
    _addresses.Add(userStreetAddress);

    return userStreetAddress;
  }
}
