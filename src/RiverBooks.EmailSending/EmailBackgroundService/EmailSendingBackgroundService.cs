using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RiverBooks.EmailSending.Services.Interfaces;

namespace RiverBooks.EmailSending;

internal class EmailSendingBackgroundService(
  ILogger<EmailSendingBackgroundService> logger,
  ISendEmailsFromOutboxService sendEmailsFromOutboxService) : BackgroundService
{
  private readonly ILogger<EmailSendingBackgroundService> _logger = logger;
  private readonly ISendEmailsFromOutboxService _sendEmailsFromOutboxService = sendEmailsFromOutboxService;

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    const int delayMilliseconds = 10_000;
    _logger.LogInformation("{ServiceName} starting...", nameof(EmailSendingBackgroundService));

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        await _sendEmailsFromOutboxService.CheckForAndSendEmails();
      }
      catch (Exception e)
      {
        _logger.LogError(e, "Error processing outbox: {message}", e.Message);
      }
      finally
      {
        await Task.Delay(delayMilliseconds, stoppingToken);
      }
    }
    
    _logger.LogInformation("{ServiceName} stopping", nameof(EmailSendingBackgroundService));
  }
}
