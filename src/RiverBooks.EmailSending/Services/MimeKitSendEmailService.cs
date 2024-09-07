using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using RiverBooks.EmailSending.Services.Interfaces;

namespace RiverBooks.EmailSending.Services;

public class MimeKitSendEmailService(ILogger<MimeKitSendEmailService> logger) : ISendEmailService
{
  private readonly ILogger<MimeKitSendEmailService> _logger = logger;

  public async Task SendEmailAsync(string to, string from, string subject, string body,
    CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Sending email to {to} from {from} with subject {subject} with body {body}", to, from,
      subject, body);
    
    using var client = new SmtpClient();
    // TODO: Read SMTP server ULR from config.
    await client.ConnectAsync("localhost", 25, false, cancellationToken);
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress(from, from));
    message.To.Add(new MailboxAddress(to, to));
    message.Subject = subject;
    message.Body = new TextPart("plain") { Text = body };
    
    await client.SendAsync(message, cancellationToken);
    _logger.LogInformation("Email sent");
    
    await client.DisconnectAsync(true, cancellationToken);
  }
}
