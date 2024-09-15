namespace RiverBooks.EmailSending.Services.Interfaces;

public interface ISendEmailsFromOutboxService
{
  Task CheckForAndSendEmails();
}
