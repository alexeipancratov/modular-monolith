using Ardalis.Result;
using MediatR;
using RiverBooks.Users.CartEndpoints.ListItems;
using RiverBooks.Users.Data.Repositories;

namespace RiverBooks.Users.UseCases;

public record ListCartItemsQuery(string EmailAddress) : IRequest<Result<IReadOnlyCollection<CartItemDto>>>;

public class ListCartItemsQueryHandler(IApplicationUserRepository userRepository)
  : IRequestHandler<ListCartItemsQuery, Result<IReadOnlyCollection<CartItemDto>>>
{
  private readonly IApplicationUserRepository _userRepository = userRepository;

  public async Task<Result<IReadOnlyCollection<CartItemDto>>> Handle(ListCartItemsQuery request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmailAsync(request.EmailAddress);
    if (user is null)
    {
      return Result.Unauthorized();
    }

    var cartItems = user.CartItems
      .Select(i => new CartItemDto(i.Id, i.BookId, i.Description, i.Quantity, i.UnitPrice))
      .ToList();

    return cartItems;
  }
}
