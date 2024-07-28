using RiverBooks.OrderProcessing.Endpoints.Models;

namespace RiverBooks.OrderProcessing.Endpoints.ListOrdersForUser;

public class ListOrdersForUserResponse
{
  public IReadOnlyCollection<OrderSummary> Orders { get; set; } = [];
}
