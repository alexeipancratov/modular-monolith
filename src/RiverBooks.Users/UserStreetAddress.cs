namespace RiverBooks.Users;

/// <summary>
/// User's address entity.
/// </summary>
public class UserStreetAddress
{
  public UserStreetAddress(string userId, Address streetAddress)
  {
    UserId = userId;
    StreetAddress = streetAddress;
  }

  public UserStreetAddress()
  {
    // EF
  }

  public Guid Id { get; private set; } = Guid.NewGuid();

  public string UserId { get; private set; } = string.Empty;

  public Address StreetAddress { get; private set; } = default!;
}
