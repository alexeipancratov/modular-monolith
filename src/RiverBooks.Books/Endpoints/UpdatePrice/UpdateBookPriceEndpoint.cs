using FastEndpoints;

namespace RiverBooks.Books.Endpoints.UpdatePrice;

internal class UpdateBookPriceEndpoint(IBookService bookService) : Endpoint<UpdateBookPriceRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;

  public override void Configure()
  {
    Post("/books/{Id}/pricehistory");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UpdateBookPriceRequest req, CancellationToken ct)
  {
    await _bookService.UpdateBookPriceAsync(req.Id, req.NewPrice);

    var updatedBook = await _bookService.GetBookByIdAsync(req.Id);

    if (updatedBook is null)
    {
      await SendNotFoundAsync(ct);
      return;
    }
    
    await SendAsync(updatedBook, 200, ct);
  }
}
