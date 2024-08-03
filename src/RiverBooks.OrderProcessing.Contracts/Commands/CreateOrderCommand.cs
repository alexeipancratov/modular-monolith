using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts.Models;

namespace RiverBooks.OrderProcessing.Contracts.Commands;

public record CreateOrderCommand(
  Guid UserId,
  Guid ShippingAddressId,
  Guid BillingAddressId,
  IReadOnlyList<OrderItemDetails> OrderItems) : IRequest<Result<OrderDetailsResponse>>;
