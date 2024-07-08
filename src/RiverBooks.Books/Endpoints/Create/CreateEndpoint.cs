using FastEndpoints;
using RiverBooks.Books.Endpoints.GetById;

namespace RiverBooks.Books.Endpoints.Create;

internal class CreateEndpoint(IBookService bookService) : Endpoint<CreateBookRequest, BookDto>
{
  private readonly IBookService _bookService = bookService;
  
  public override void Configure()
  {
    Post("/books");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
  {
    var newBookDto = new BookDto(req.Id ?? Guid.NewGuid(), req.Title, req.Author, req.Price);

    await _bookService.CreateBookAsync(newBookDto);

    await SendCreatedAtAsync<GetBookByIdEndpoint>(new { newBookDto.Id }, newBookDto, cancellation: ct);
  }
}
