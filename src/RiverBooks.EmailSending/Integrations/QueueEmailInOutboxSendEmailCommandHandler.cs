using Ardalis.Result;
using MediatR;
using MongoDB.Driver;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.EmailSending.Domain;

namespace RiverBooks.EmailSending.Integrations;

// This file adheres to the Vertical Slice architecture, i.e., all in one file.
internal interface IQueueEmailsInOutboxService
{
  Task QueueEmailForSending(EmailOutboxEntity entity); 
}

internal class MongoDbQueueEmailOutboxService(IMongoCollection<EmailOutboxEntity> emailCollection)
  : IQueueEmailsInOutboxService
{
  private readonly IMongoCollection<EmailOutboxEntity> _emailCollection = emailCollection;

  public async Task QueueEmailForSending(EmailOutboxEntity entity)
  {
    await _emailCollection.InsertOneAsync(entity);
  }
}

internal class QueueEmailInOutboxSendEmailCommandHandler(IQueueEmailsInOutboxService outboxService)
  : IRequestHandler<SendEmailCommand, Result<Guid>>
{
  private readonly IQueueEmailsInOutboxService _outboxService = outboxService;

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
