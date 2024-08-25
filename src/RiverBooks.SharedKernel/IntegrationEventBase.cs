using MediatR;

namespace RiverBooks.SharedKernel;

/// <summary>
/// TODO: Define this type in a common libary instead of duplicating it here.
/// </summary>
public abstract record IntegrationEventBase : INotification
{
  public DateTimeOffset DateTimeOffset { get; set; } = DateTimeOffset.UtcNow;
}
