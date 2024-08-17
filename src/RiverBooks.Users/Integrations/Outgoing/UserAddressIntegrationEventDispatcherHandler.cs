using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.Contracts.IntegrationEvents;
using RiverBooks.Users.DomainEvents;

namespace RiverBooks.Users.Integrations.Outgoing;

/// <summary>
/// Intercepts <see cref="AddressAddedDomainEvent"/> and publishes an integration event analog.
/// </summary>
internal class UserAddressIntegrationEventDispatcherHandler(
  IMediator mediator,
  ILogger<UserAddressIntegrationEventDispatcherHandler> logger)
  : INotificationHandler<AddressAddedDomainEvent>
{
  private readonly IMediator _mediator = mediator;
  private readonly ILogger<UserAddressIntegrationEventDispatcherHandler> _logger = logger;

  public async Task Handle(AddressAddedDomainEvent notification, CancellationToken cancellationToken)
  {
    Guid userId = Guid.Parse(notification.NewAddress.UserId);
    var newAddress = notification.NewAddress;

    var addressDetails = new UserAddressDetails(
      userId,
      newAddress.Id,
      newAddress.StreetAddress.Street1,
      newAddress.StreetAddress.Street2,
      newAddress.StreetAddress.City,
      newAddress.StreetAddress.State,
      newAddress.StreetAddress.PostalCode,
      newAddress.StreetAddress.Country);

    await _mediator.Publish(new NewUserAddressAddedIntegrationEvent(addressDetails), cancellationToken);
    
    _logger.LogInformation("[DE Handler]New address integration event sent for {user} with address {address}",
      newAddress.UserId,
      newAddress.StreetAddress);
  }
}
