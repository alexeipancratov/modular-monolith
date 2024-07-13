using RiverBooks.Books.Data;

namespace RiverBooks.Books;

internal class BookService(IBookRepository bookRepository) : IBookService
{
  private readonly IBookRepository _bookRepository = bookRepository;
  
  public async Task<IReadOnlyList<BookDto>> ListBooksAsync()
  {
    var books = (await _bookRepository.GetAllAsync())
      .Select(b => new BookDto(b.Id, b.Title, b.Author, b.Price))
      .ToList();

    return books;
  }

  public async Task<BookDto?> GetBookByIdAsync(Guid id)
  {
    var book = await _bookRepository.GetByIdAsync(id);

    return book is not null
      ? new BookDto(book.Id, book.Title, book.Author, book.Price)
      : null;
  }

  public async Task CreateBookAsync(BookDto newBook)
  {
    var book = new Book(newBook.Id, newBook.Title, newBook.Author, newBook.Price);

    await _bookRepository.AddAsync(book);
    await _bookRepository.SaveChangesAsync();
  }

  public async Task DeleteBookAsync(Guid id)
  {
    var book = await _bookRepository.GetByIdAsync(id);

    if (book is not null)
    {
      await _bookRepository.DeleteAsync(book);
      await _bookRepository.SaveChangesAsync();
    }
  }

  public async Task UpdateBookPriceAsync(Guid id, decimal price)
  {
    var book = await _bookRepository.GetByIdAsync(id);
    
    // TODO: handle not found case
    book!.UpdatePrice(price);

    await _bookRepository.SaveChangesAsync();
  }
}
