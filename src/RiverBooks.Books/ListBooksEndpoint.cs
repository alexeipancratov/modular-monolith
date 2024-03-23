using FastEndpoints;

namespace RiverBooks.Books;

// ASP.NET Core version.
// public static class BooksEndpoints
// {
//     public static void MapBookEndpoints(this WebApplication app)
//     {
//         app.MapGet("/books", (IBookService service) => service.ListBooks());
//     }
// }

internal class ListBooksEndpoint(IBookService bookService) : EndpointWithoutRequest<ListBooksResponse>
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new ListBooksResponse
        {
            Books = bookService.ListBooks()
        }, 200, ct);
    }

    public override void Configure()
    {
        Get("/books");
        AllowAnonymous();
    }
}

public class ListBooksResponse
{
    public IReadOnlyCollection<BookDto> Books { get; set; }
}