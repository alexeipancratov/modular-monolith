using Ardalis.Result;
using MediatR;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.EmailSending.Domain;
using RiverBooks.EmailSending.Services.Interfaces;

namespace RiverBooks.EmailSending.Integrations;

internal class QueueEmailInOutboxSendEmailCommandHandler(IOutboxService outboxService)
  : IRequestHandler<SendEmailCommand, Result<Guid>>
{
  private readonly IOutboxService _outboxService = outboxService;

  public async Task<Result<Guid>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
  {
    var newEntity = new EmailOutboxEntity
    {
      Body = request.Body, Subject = request.Subject, To = request.To, From = request.From
    };
    
    await _outboxService.QueueEmailForSending(newEntity);

    return newEntity.Id;
  }
}
