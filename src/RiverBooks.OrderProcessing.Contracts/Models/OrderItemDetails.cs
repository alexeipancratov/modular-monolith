namespace RiverBooks.OrderProcessing.Contracts.Models;

public record OrderItemDetails(Guid BookId, int Quantity, decimal UnitPrice, string Description);
