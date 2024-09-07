using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.Contracts.Queries;

public record GetUserAddressDetailsByIdQuery(Guid AddressId) : IRequest<Result<UserAddressDetails>>;
