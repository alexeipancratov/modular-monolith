using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books.Data;

internal class EfBookRepository(BookDbContext dbContext) : IBookRepository
{
  private readonly BookDbContext _dbContext = dbContext;

  public async Task<Book?> GetByIdAsync(Guid id)
  {
    return await _dbContext.Books.FindAsync(id);
  }

  public async Task<IReadOnlyList<Book>> GetAllAsync()
  {
    return await _dbContext.Books.ToArrayAsync();
  }

  public async Task AddAsync(Book book)
  {
    await _dbContext.Books.AddAsync(book);
  }

  public Task DeleteAsync(Book book)
  {
    _dbContext.Books.Remove(book);
    return Task.CompletedTask;
  }

  public Task SaveChangesAsync()
  {
    return _dbContext.SaveChangesAsync();
  }
}
