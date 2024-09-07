using Ardalis.Result;
using MediatR;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.EmailSending.Services.Interfaces;

namespace RiverBooks.EmailSending.Integrations;

internal class SendEmailCommandHandler(ISendEmailService sendEmailService)
  : IRequestHandler<SendEmailCommand, Result<Guid>>
{
  private readonly ISendEmailService _sendEmailService = sendEmailService;

  public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
  {
    await _sendEmailService.SendEmailAsync(request.To, request.From, request.Subject, request.Body, cancellationToken);

    return Guid.Empty;
  }
}
