using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data.Repositories;

internal class EfApplicationUserRepository(UsersDbContext dbContext) : IApplicationUserRepository
{
  private readonly UsersDbContext _dbContext = dbContext;

  public Task<ApplicationUser?> GetUserWithCartByEmailAsync(string email)
  {
    return _dbContext.ApplicationUsers
      .Include(u => u.CartItems)
      .SingleOrDefaultAsync(u => u.Email == email);
  }

  public Task SaveChangesAsync()
  {
    return _dbContext.SaveChangesAsync();
  }
}
