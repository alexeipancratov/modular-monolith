using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RiverBooks.EmailSending.Domain;

public class EmailOutboxEntity
{
  [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
  public Guid Id { get; set; } = Guid.NewGuid();

  public string To { get; set; } = string.Empty;

  public string From { get; set; } = string.Empty;
  
  public string Subject { get; set; } = string.Empty;
  
  public string Body { get; set; } = string.Empty;

  public DateTime? DateTimeUtcProcessed { get; set; } = null!;
}
