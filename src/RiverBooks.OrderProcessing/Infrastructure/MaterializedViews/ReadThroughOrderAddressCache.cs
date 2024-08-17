using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.OrderProcessing.Interfaces;
using RiverBooks.Users.Contracts.Queries;

namespace RiverBooks.OrderProcessing.Infrastructure.MaterializedViews;

/// <summary>
/// An implementation of the OrderAddress cache which can make a call to the underlying service if data is missing
/// in the cache.
/// </summary>
/// <remarks>Decorator pattern is being used here.</remarks>
internal class ReadThroughOrderAddressCache(
  RedisOrderAddressCache redisCache,
  IMediator mediator,
  ILogger<ReadThroughOrderAddressCache> logger)
  : IOrderAddressCache
{
  private readonly RedisOrderAddressCache _redisCache = redisCache;
  private readonly IMediator _mediator = mediator;
  private readonly ILogger<ReadThroughOrderAddressCache> _logger = logger;

  public async Task<Result<OrderAddress>> GetByIdAsync(Guid addressId)
  {
    var result = await _redisCache.GetByIdAsync(addressId);

    if (result.IsSuccess)
      return result;

    if (result.Status != ResultStatus.NotFound)
    {
      return result;
    }

    // fetch data from source directly
    _logger.LogInformation("Address {id} not found; fetching from source", addressId);

    var queryResult = await _mediator.Send(new UserAddressDetailsByIdQuery(addressId));

    if (!queryResult.IsSuccess)
    {
      return result;
    }

    var dto = queryResult.Value;
    var address = new Address(
      dto.Street1,
      dto.Street2,
      dto.City,
      dto.State,
      dto.PostalCode,
      dto.Country);
    var orderAddress = new OrderAddress(addressId, address);
    await StoreAsync(orderAddress);

    return orderAddress;
  }

  public Task<Result> StoreAsync(OrderAddress orderAddress)
  {
    return _redisCache.StoreAsync(orderAddress);
  }
}
