using RiverBooks.EmailSending.Domain;

namespace RiverBooks.EmailSending.Endpoints.ListEmails;

public class ListEmailResponse
{
  public int Count { get; set; }

  // TODO: Return list of DTOs
  public IReadOnlyCollection<EmailOutboxEntity> Emails { get; internal set; } = new List<EmailOutboxEntity>();
}
