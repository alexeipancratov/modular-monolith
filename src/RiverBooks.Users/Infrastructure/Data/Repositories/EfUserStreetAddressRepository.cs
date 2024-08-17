using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Infrastructure.Data.Repositories;

public class EfUserStreetAddressRepository(UsersDbContext dbContext) : IReadOnlyUserStreetAddressRepository
{
  private readonly UsersDbContext _dbContext = dbContext;

  public Task<UserStreetAddress?> GetByIdAsync(Guid userStreetAddressId)
  {
    return _dbContext.UserStreetAddresses
      .SingleOrDefaultAsync(a => a.Id == userStreetAddressId);
  }
}
