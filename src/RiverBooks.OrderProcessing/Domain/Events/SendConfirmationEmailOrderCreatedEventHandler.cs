using MediatR;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.Users.Contracts.Queries;

namespace RiverBooks.OrderProcessing.Domain.Events;

internal class SendConfirmationEmailOrderCreatedEventHandler(IMediator mediator) : INotificationHandler<OrderCreatedEvent>
{
  private readonly IMediator _mediator = mediator;

  public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
  {
    var userResult = await _mediator.Send(new GetUserDetailsByIdQuery(notification.Order.UserId), cancellationToken);

    if (!userResult.IsSuccess)
    {
      // TODO: Log the error
      return;
    }
    
    var command = new SendEmailCommand(
      To: userResult.Value.EmailAddress,
      From: "noreply@test.com",
      Subject: "Your RiverBook purchase",
      Body: $"You bought {notification.Order.OrderItems.Count} items");
    Guid emailId = await _mediator.Send(command, cancellationToken);
    
    // TODO: Do something this emailId
  }
}
