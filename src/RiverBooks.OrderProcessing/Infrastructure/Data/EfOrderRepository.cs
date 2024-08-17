using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Interfaces;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

internal class EfOrderRepository(OrderProcessingDbContext dbContext) : IOrderRepository
{
  private readonly OrderProcessingDbContext _dbContext = dbContext;
  
  public async Task<IReadOnlyCollection<Order>> GetAllAsync()
  {
    // TODO: Use Specification here instead of including related records explicitly all the time.
    // (e.g., Ardalis.Specification).
    return await _dbContext.Orders
      .Include(o => o.OrderItems)
      .ToListAsync();
  }

  public async Task AddAsync(Order order)
  {
    await _dbContext.Orders.AddAsync(order);
  }

  public Task SaveChangesAsync()
  {
    return _dbContext.SaveChangesAsync();
  }
}
