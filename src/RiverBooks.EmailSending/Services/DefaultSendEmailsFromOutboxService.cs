using Ardalis.Result;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RiverBooks.EmailSending.Domain;
using RiverBooks.EmailSending.Services.Interfaces;

namespace RiverBooks.EmailSending.Services;

internal class DefaultSendEmailsFromOutboxService(
  IOutboxService outboxService,
  ISendEmailService sendEmailService,
  // TODO: Abstract this collection behind a repository.
  IMongoCollection<EmailOutboxEntity> emailCollection,
  ILogger<DefaultSendEmailsFromOutboxService> logger) : ISendEmailsFromOutboxService
{
  private readonly IOutboxService _outboxService = outboxService;
  private readonly ISendEmailService _sendEmailService = sendEmailService;
  private readonly IMongoCollection<EmailOutboxEntity> _emailCollection = emailCollection;
  private readonly ILogger<DefaultSendEmailsFromOutboxService> _logger = logger;

  public async Task CheckForAndSendEmails()
  {
    try
    {
      var result = await _outboxService.GetUnprocessedEmailEntity();

      if (result.Status == ResultStatus.NotFound)
      {
        return;
      }

      var emailEntity = result.Value;

      await _sendEmailService.SendEmailAsync(emailEntity.To, emailEntity.From, emailEntity.Subject, emailEntity.Body);

      var updateFilter = Builders<EmailOutboxEntity>
        .Filter.Eq(x => x.Id, emailEntity.Id);
      var update = Builders<EmailOutboxEntity>
        .Update.Set(nameof(EmailOutboxEntity.DateTimeUtcProcessed), DateTime.UtcNow);
      var updateResult = await _emailCollection.UpdateOneAsync(updateFilter, update);
      
      _logger.LogInformation("Processed {Count} email records", updateResult.MatchedCount);
    }
    finally
    {
      _logger.LogInformation("Sleeping...");
    }
  }
}
