namespace RiverBooks.EmailSending.Services.Interfaces;

internal interface ISendEmailService
{
  Task SendEmailAsync(string to, string from, string subject, string body, CancellationToken cancellationToken = default);
}
