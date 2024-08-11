namespace RiverBooks.Users.DomainEvents.Infrastructure;

public interface IDomainEventDispatcher
{
  Task DispatchAndClearEvents(IReadOnlyCollection<IHaveDomainEvents> entitiesWithEvents);
}
