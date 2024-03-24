namespace RiverBooks.Books;

internal class BookService : IBookService
{
    public IReadOnlyCollection<BookDto> ListBooks()
    {
        return
        [
            new BookDto(Guid.NewGuid(), "The Fellowship of the Ring", "J.R.R. Tolkien"),
            new BookDto(Guid.NewGuid(), "The Two Towers", "J.R.R. Tolkien"),
            new BookDto(Guid.NewGuid(), "The Return of the King", "J.R.R. Tolkien"),
            new BookDto(Guid.NewGuid(), "The Hobbit", "J.R.R. Tolkien"),
            new BookDto(Guid.NewGuid(), "Master and Margarita", "Mikhail Bulgakov")
        ];
    }
}