using Ardalis.Result;
using RiverBooks.EmailSending.Domain;

namespace RiverBooks.EmailSending.Services.Interfaces;

internal interface IOutboxService
{
  Task QueueEmailForSending(EmailOutboxEntity entity);

  Task<Result<EmailOutboxEntity>> GetUnprocessedEmailEntity();
}
