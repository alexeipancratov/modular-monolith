using Ardalis.Result;

namespace RiverBooks.OrderProcessing.MaterializedViews;

internal interface IOrderAddressCache
{
  Task<Result<OrderAddress>> GetByIdAsync(Guid addressId);
  
  Task<Result> StoreAsync(OrderAddress orderAddress);
}
