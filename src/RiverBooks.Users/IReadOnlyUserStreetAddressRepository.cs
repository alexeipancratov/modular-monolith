namespace RiverBooks.Users;

internal interface IReadOnlyUserStreetAddressRepository
{
  Task<UserStreetAddress?> GetByIdAsync(Guid userStreetAddressId);
}
