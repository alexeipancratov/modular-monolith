using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Interfaces;

internal interface IOrderRepository
{
  Task<IReadOnlyCollection<Order>> GetAllAsync();

  Task AddAsync(Order order);

  Task SaveChangesAsync();
}
