using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.Contracts.Queries;
using RiverBooks.Users.Interfaces;

namespace RiverBooks.Users.Integrations.Outgoing;

internal class GetUserAddressDetailsByIdQueryHandler(IReadOnlyUserStreetAddressRepository addressRepository)
  : IRequestHandler<GetUserAddressDetailsByIdQuery, Result<UserAddressDetails>>
{
  private readonly IReadOnlyUserStreetAddressRepository _addressRepository = addressRepository;

  public async Task<Result<UserAddressDetails>> Handle(GetUserAddressDetailsByIdQuery request, CancellationToken cancellationToken)
  {
    var address = await _addressRepository.GetByIdAsync(request.AddressId);

    if (address is null)
      return Result.NotFound();

    var userId = Guid.Parse(address.UserId);

    var details = new UserAddressDetails(userId,
      address.Id,
      address.StreetAddress.Street1,
      address.StreetAddress.Street2,
      address.StreetAddress.City,
      address.StreetAddress.State,
      address.StreetAddress.PostalCode,
      address.StreetAddress.Country);

    return details;
  }
}
