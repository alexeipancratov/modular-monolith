using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Books.Contracts;
using RiverBooks.OrderProcessing.Contracts.Events;

namespace RiverBooks.Reporting.Integrations;

public class NewOrderCreatedIngestionHandler(
  ILogger<NewOrderCreatedIngestionHandler> logger,
  OrderIngestionService orderIngestionService,
  IMediator mediator) : INotificationHandler<OrderCreatedIntegrationEvent>
{
  private readonly ILogger<NewOrderCreatedIngestionHandler> _logger = logger;
  private readonly OrderIngestionService _orderIngestionService = orderIngestionService;
  private readonly IMediator _mediator = mediator;

  public async Task Handle(OrderCreatedIntegrationEvent notification, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Handling order created event to populate reporting database.");

    var orderItems = notification.OrderDetails.OrderItems;
    var year = notification.OrderDetails.DateCreated.Year;
    var month = notification.OrderDetails.DateCreated.Month;

    foreach (var orderItem in orderItems)
    {
      // TODO: Implement materialized view or other cache to retrieve this data
      // because it doesn't change often (instead of hitting DB each time).
      var bookDetailsQuery = new GetBookDetailsQuery(orderItem.BookId);
      var result = await _mediator.Send(bookDetailsQuery, cancellationToken);

      if (!result.IsSuccess)
      {
        _logger.LogWarning("Issue loading book details for {BookId}", orderItem.BookId);
        continue;
      }

      string author = result.Value.Author;
      string title = result.Value.Title;

      var sale = new BookSale
      {
        Author = author,
        BookId = orderItem.BookId,
        Month = month,
        Title = title,
        Year = year,
        TotalSales = orderItem.Quantity * orderItem.UnitPrice,
        UnitsSold = orderItem.Quantity
      };

      await _orderIngestionService.AddOrUpdateMonthlyBookSalesAsync(sale);
    }
  }
}
