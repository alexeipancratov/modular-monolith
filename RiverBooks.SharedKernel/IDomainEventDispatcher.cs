namespace RiverBooks.SharedKernel;

public interface IDomainEventDispatcher
{
  Task DispatchAndClearEvents(IReadOnlyCollection<IHaveDomainEvents> entitiesWithEvents);
}
