namespace RiverBooks.Users;

public interface IHaveDomainEvents
{
  public IReadOnlyCollection<DomainEventBase> DomainEvents { get; }

  void ClearDomainEvents();
}
