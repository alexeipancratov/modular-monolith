using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data.Repositories;

public class EfUserStreetAddressRepository(UsersDbContext dbContext) : IReadOnlyUserStreetAddressRepository
{
  private readonly UsersDbContext _dbContext = dbContext;

  public Task<UserStreetAddress?> GetByIdAsync(Guid userStreetAddressId)
  {
    return _dbContext.UserStreetAddresses
      .SingleOrDefaultAsync(a => a.Id == userStreetAddressId);
  }
}
