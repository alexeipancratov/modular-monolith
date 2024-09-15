using FastEndpoints;
using MongoDB.Driver;
using RiverBooks.EmailSending.Domain;

namespace RiverBooks.EmailSending.Endpoints.ListEmails;

// NOTE: Ideally we would've implemented a use case here,
// but since it's a debugging endpoint, we injected MongoCollection directly.
internal class ListEmailsEndpoint(IMongoCollection<EmailOutboxEntity> emailCollection) : EndpointWithoutRequest<ListEmailResponse>
{
  private readonly IMongoCollection<EmailOutboxEntity> _emailCollection = emailCollection;

  public override void Configure()
  {
    Get("/emails");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    // TODO: Implement paging.
    var filter = Builders<EmailOutboxEntity>.Filter.Empty;
    var emailEntities = await _emailCollection.Find(filter).ToListAsync(ct);

    Response = new ListEmailResponse { Count = emailEntities.Count, Emails = emailEntities };
  }
}
