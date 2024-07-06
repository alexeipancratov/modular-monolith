namespace RiverBooks.Books;

internal interface IReadOnlyBookRepository
{
  Task<Book?> GetByIdAsync(Guid id);
  Task<IReadOnlyList<Book>> GetAllAsync();
}
