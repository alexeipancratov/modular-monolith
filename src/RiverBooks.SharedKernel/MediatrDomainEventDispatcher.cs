using MediatR;

namespace RiverBooks.SharedKernel;

public class MediatrDomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
  private readonly IMediator _mediator = mediator;

  public async Task DispatchAndClearEvents(IReadOnlyCollection<IHaveDomainEvents> entitiesWithEvents)
  {
    foreach (var entity in entitiesWithEvents)
    {
      // We need to copy events since we'll clear this collection in the next step.
      var entityDomainEvents = entity.DomainEvents.ToArray();
      entity.ClearDomainEvents();
      foreach (var entityDomainEvent in entityDomainEvents)
      {
        // TODO: Check why .ConfigureAwait(false) might be useful here.
        await _mediator.Publish(entityDomainEvent);
      }
    }
  }
}
