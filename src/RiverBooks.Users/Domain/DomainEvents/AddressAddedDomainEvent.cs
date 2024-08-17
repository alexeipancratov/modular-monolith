using RiverBooks.SharedKernel;

namespace RiverBooks.Users.Domain.DomainEvents;

// It's okay for domain events to provide only basic info like IDs since more data can be cheaply retrieved.
// Integration events should have as much data as possible.
internal sealed class AddressAddedDomainEvent : DomainEventBase
{
  public UserStreetAddress NewAddress { get; set; }
  
  public AddressAddedDomainEvent(UserStreetAddress newAddress)
  {
    NewAddress = newAddress;
  }
}
