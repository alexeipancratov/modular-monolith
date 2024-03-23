namespace RiverBooks.Books;

internal interface IBookService
{
    IReadOnlyCollection<BookDto> ListBooks();
}