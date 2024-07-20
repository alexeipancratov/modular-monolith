using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Data.Repositories;

namespace RiverBooks.Users.UseCases;

public record AddItemToCartCommand(Guid BookId, int Quantity, string EmailAddres) : IRequest<Result>;

public class AddItemToCartCommandHandler(IApplicationUserRepository userRepository)
  : IRequestHandler<AddItemToCartCommand, Result>
{
  private readonly IApplicationUserRepository _userRepository = userRepository;

  public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmailAsync(request.EmailAddres);
    if (user is null)
    {
      return Result.Unauthorized();
    }

    // TODO: Get Description from the Books module.
    var newCartItem = new CartItem(request.BookId, "description", request.Quantity, 1.00m);

    user.AddCardItem(newCartItem);

    await _userRepository.SaveChangesAsync();
    
    return Result.Success();
  }
}
