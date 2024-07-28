namespace RiverBooks.OrderProcessing;

internal interface IOrderRepository
{
  Task<IReadOnlyCollection<Order>> GetAllAsync();

  Task AddAsync(Order order);

  Task SaveChangesAsync();
}
