using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.MaterializedViews;
using RiverBooks.Users.Contracts.IntegrationEvents;

namespace RiverBooks.OrderProcessing.Integrations.IncomingEvents;

internal class AddressCacheUpdatingNewUserAddressHandler(
  IOrderAddressCache orderAddressCache,
  ILogger<AddressCacheUpdatingNewUserAddressHandler> logger) : INotificationHandler<NewUserAddressAddedIntegrationEvent>
{
  private readonly IOrderAddressCache _orderAddressCache = orderAddressCache;
  private readonly ILogger<AddressCacheUpdatingNewUserAddressHandler> _logger = logger;

  public async Task Handle(NewUserAddressAddedIntegrationEvent notification, CancellationToken cancellationToken)
  {
    var orderAddress = new OrderAddress(notification.Details.AddressId,
      new Address(notification.Details.Street1,
        notification.Details.Street2,
        notification.Details.City,
        notification.Details.State,
        notification.Details.PostalCode,
        notification.Details.Country));

    await _orderAddressCache.StoreAsync(orderAddress);

    _logger.LogInformation("Cache updated with new address {address} for user {userId}",
      orderAddress,
      notification.Details.UserId);
  }
}
