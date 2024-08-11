using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.DomainEvents;

namespace RiverBooks.Users;

public class ApplicationUser : IdentityUser, IHaveDomainEvents
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
    
    // Raise a domain event
    var domainEvent = new AddressAddedDomainEvent(userStreetAddress);
    RegisterDomainEvent(domainEvent);

    return userStreetAddress;
  }

  private readonly List<DomainEventBase> _domainEvents = [];

  [NotMapped]
  public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
  
  void IHaveDomainEvents.ClearDomainEvents() => _domainEvents.Clear();
}
