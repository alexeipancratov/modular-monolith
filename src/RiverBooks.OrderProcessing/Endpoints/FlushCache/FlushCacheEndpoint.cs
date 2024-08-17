using FastEndpoints;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RiverBooks.OrderProcessing.Endpoints.FlushCache;

/// <summary>
/// Used for testing only.
/// </summary>
public class FlushCacheEndpoint : EndpointWithoutRequest
{
  private readonly ILogger<FlushCacheEndpoint> _logger;
  private readonly IDatabase _db;

  public FlushCacheEndpoint(ILogger<FlushCacheEndpoint> logger)
  {
    _logger = logger;
    // TODO: Use DI.
    var redis = ConnectionMultiplexer.Connect("localhost"); // TODO: get server from config.
    _db = redis.GetDatabase();
  }

  public override void Configure()
  {
    Post("/flushcache");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    await _db.ExecuteAsync("FLUSHDB");
    _logger.LogWarning("FLUSHED CACHE FOR {db}", "REDIS");
  }
}
