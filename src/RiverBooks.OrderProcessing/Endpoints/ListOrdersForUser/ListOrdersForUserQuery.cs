using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Endpoints.Models;

namespace RiverBooks.OrderProcessing.Endpoints.ListOrdersForUser;

public record ListOrdersForUserQuery(string EmailAddress) : IRequest<Result<IReadOnlyList<OrderSummary>>>;

internal class ListOrdersForUserQueryHandler(IOrderRepository orderRepository) : IRequestHandler<ListOrdersForUserQuery, Result<IReadOnlyList<OrderSummary>>>
{
  private readonly IOrderRepository _orderRepository = orderRepository;

  public async Task<Result<IReadOnlyList<OrderSummary>>> Handle(ListOrdersForUserQuery request, CancellationToken cancellationToken)
  {
    // lookup UserId by Email
    
    // TODO: filter by User
    var orders = await _orderRepository.GetAllAsync();

    var summaries = orders
      .Select(o => new OrderSummary
      {
        DateCreated = o.DateCreated,
        OrderId = o.Id,
        UserId = o.UserId,
        Total = o.OrderItems.Sum(i => i.UnitPrice) // need to .Include OrderItems
      })
      .ToArray();

    return summaries;
  }
}
