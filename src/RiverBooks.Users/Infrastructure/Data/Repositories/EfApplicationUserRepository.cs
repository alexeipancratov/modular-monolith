using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data.Repositories;

internal class EfApplicationUserRepository(UsersDbContext dbContext) : IApplicationUserRepository
{
  private readonly UsersDbContext _dbContext = dbContext;

  public Task<ApplicationUser?> GetUserWithCartByEmailAsync(string email)
  {
    return _dbContext.ApplicationUsers
      .Include(u => u.CartItems)
      .SingleOrDefaultAsync(u => u.Email == email);
  }

  public Task<ApplicationUser?> GetUserWithAddressesByEmailAsync(string email)
  {
    return _dbContext.ApplicationUsers
      .Include(u => u.Addresses)
      .SingleOrDefaultAsync(u => u.Email == email);
  }

  public Task<ApplicationUser?> GetUserByIdAsync(Guid userId)
  {
    return _dbContext.ApplicationUsers
      .SingleOrDefaultAsync(user => user.Id == userId.ToString());
  }

  public Task SaveChangesAsync()
  {
    return _dbContext.SaveChangesAsync();
  }
}
