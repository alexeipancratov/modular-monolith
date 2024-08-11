using MediatR;

namespace RiverBooks.Users.Contracts.IntegrationEvents;

/// <summary>
/// TODO: Define this type in a common libary instead of duplicating it here.
/// </summary>
public abstract record IntegrationEventBase : INotification
{
  public DateTimeOffset DateTimeOffset { get; set; } = DateTimeOffset.UtcNow;
}
