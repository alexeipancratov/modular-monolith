namespace RiverBooks.SharedKernel;

public interface IHaveDomainEvents
{
  public IReadOnlyCollection<DomainEventBase> DomainEvents { get; }

  void ClearDomainEvents();
}
