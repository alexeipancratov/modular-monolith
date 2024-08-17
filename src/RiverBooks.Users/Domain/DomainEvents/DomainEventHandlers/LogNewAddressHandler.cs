using MediatR;
using Microsoft.Extensions.Logging;

namespace RiverBooks.Users.Domain.DomainEvents.DomainEventHandlers;

internal class LogNewAddressHandler(ILogger<LogNewAddressHandler> logger) : INotificationHandler<AddressAddedDomainEvent>
{
  private readonly ILogger<LogNewAddressHandler> _logger = logger;

  public Task Handle(AddressAddedDomainEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("[DE Handler] New address added to user {usedId}: {address}", notification.NewAddress.UserId,
      notification.NewAddress.StreetAddress);

    return Task.CompletedTask;
  }
}
