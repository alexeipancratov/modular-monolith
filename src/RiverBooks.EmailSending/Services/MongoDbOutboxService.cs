using Ardalis.Result;
using MongoDB.Driver;
using RiverBooks.EmailSending.Domain;
using RiverBooks.EmailSending.Services.Interfaces;

namespace RiverBooks.EmailSending.Services;

public class MongoDbOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection) : IOutboxService
{
  private readonly IMongoCollection<EmailOutboxEntity> _emailCollection = emailCollection;

  public async Task QueueEmailForSending(EmailOutboxEntity entity)
  {
    await _emailCollection.InsertOneAsync(entity);
  }

  public async Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity()
  {
    var filter = Builders<EmailOutboxEntity>.Filter.Eq(entity => entity.DateTimeUtcProcessed, null);
    var unsentEmailEntity = await _emailCollection.Find(filter).FirstOrDefaultAsync();

    if (unsentEmailEntity == null)
    {
      return Result.NotFound();
    }

    return unsentEmailEntity;
  }
}
