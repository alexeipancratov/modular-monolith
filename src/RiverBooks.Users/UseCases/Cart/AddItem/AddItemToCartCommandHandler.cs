using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.Books.Contracts;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.UseCases.Cart.AddItem;

public class AddItemToCartCommandHandler(
  IApplicationUserRepository userRepository,
  IMediator mediator,
  ILogger<AddItemToCartCommandHandler> logger)
  : IRequestHandler<AddItemToCartCommand, Result>
{
  private readonly IApplicationUserRepository _userRepository = userRepository;
  private readonly IMediator _mediator = mediator;
  private readonly ILogger<AddItemToCartCommandHandler> _logger = logger;

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
    
    _logger.LogInformation("Added an item to the cart {description}", newCartItem.Description);
    
    return Result.Success();
  }
}
