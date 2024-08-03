using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;

namespace RiverBooks.Users.UseCases.Cart.AddItem;

public class AddItemToCartCommandHandler(IApplicationUserRepository userRepository, IMediator mediator)
  : IRequestHandler<AddItemToCartCommand, Result>
{
  private readonly IApplicationUserRepository _userRepository = userRepository;
  private readonly IMediator _mediator = mediator;

  public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetUserWithCartByEmailAsync(request.EmailAddres);
    if (user is null)
    {
      return Result.Unauthorized();
    }

    // Get Description from the Books module.
    var bookDetailsResult = await _mediator.Send(new GetBookDetailsQuery(request.BookId), cancellationToken);
    if (bookDetailsResult.Status == ResultStatus.NotFound)
    {
      return Result.NotFound();
    }

    var bookDetails = bookDetailsResult.Value;
    var description = $"{bookDetails.Title} by {bookDetails.Author}";
    
    var newCartItem = new CartItem(request.BookId, description, request.Quantity, bookDetails.Price);

    user.AddCardItem(newCartItem);

    await _userRepository.SaveChangesAsync();
    
    return Result.Success();
  }
}
