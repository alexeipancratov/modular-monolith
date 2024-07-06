namespace RiverBooks.Books;

internal interface IBookService
{
  Task<IReadOnlyList<BookDto>> ListBooksAsync();

  Task<BookDto?> GetBookByIdAsync(Guid id);

  Task CreateBookAsync(BookDto newBook);

  Task DeleteBookAsync(Guid id);

  Task UpdateBookPriceAsync(Guid id, decimal price);
}
