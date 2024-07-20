using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;

namespace RiverBooks.Books.Integrations;

internal class GetBookDetailsQueryHandler(IBookService bookService)
  : IRequestHandler<GetBookDetailsQuery, Result<BookDetailsResponse>>
{
  private readonly IBookService _bookService = bookService;

  public async Task<Result<BookDetailsResponse>> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
  {
    var book = await _bookService.GetBookByIdAsync(request.BookId);
    if (book is null)
    {
      return Result.NotFound();
    }

    var response = new BookDetailsResponse(book.Id, book.Title, book.Author, book.Price);

    return response;
  }
}
