using FastEndpoints;

namespace RiverBooks.Books.Endpoints.GetById;

internal class GetBookByIdEndpoint(IBookService bookService) : Endpoint<GetBookByIdRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;
  
  public override void Configure()
  {
    Get("/books/{Id}");
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetBookByIdRequest req, CancellationToken ct)
  {
    var book = await _bookService.GetBookByIdAsync(req.Id);

    if (book is null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await SendAsync(book, 200, ct);
  }
}
