using Ardalis.Result;
using MediatR;
using RiverBooks.Users.CartEndpoints.ListItems;

namespace RiverBooks.Users.UseCases.Cart.ListItems;

public record ListCartItemsQuery(string EmailAddress) : IRequest<Result<IReadOnlyCollection<CartItemDto>>>;
