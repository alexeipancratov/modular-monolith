using System.Text.Json;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RiverBooks.OrderProcessing.MaterializedViews;

internal class RedisOrderAddressCache : IOrderAddressCache
{
  private readonly ILogger<RedisOrderAddressCache> _logger;
  private readonly IDatabase _db;

  public RedisOrderAddressCache(ILogger<RedisOrderAddressCache> logger)
  {
    _logger = logger;
    
    // TODO: get server conn string from the config.
    // TODO: Reuse connection between requests (as per https://stackexchange.github.io/StackExchange.Redis/Basics.html).
    var redis = ConnectionMultiplexer.Connect("localhost");
    _db = redis.GetDatabase();
  }
  
  public async Task<Result<OrderAddress>> GetByIdAsync(Guid addressId)
  {
    string? fetchedJson = await _db.StringGetAsync(addressId.ToString());

    if (fetchedJson is null)
    {
      _logger.LogWarning("Address {id} in DB {db}.", addressId, "REDIS");
      return Result.NotFound();
    }

    var address = JsonSerializer.Deserialize<OrderAddress>(fetchedJson);

    if (address is null)
      return Result.NotFound();
    
    _logger.LogInformation("Address {id} returned from {db}", addressId, "REDIS");
    
    return Result.Success(address);
  }

  public async Task<Result> StoreAsync(OrderAddress orderAddress)
  {
    string key = orderAddress.Id.ToString();
    var addressJson = JsonSerializer.Serialize(orderAddress);
    
    await _db.StringSetAsync(key, addressJson);
    _logger.LogInformation("Address {id} stored in {db}", key, "REDIS");
    
    return Result.Success();
  }
}
