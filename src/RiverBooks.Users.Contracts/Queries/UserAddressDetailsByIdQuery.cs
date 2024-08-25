using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.Contracts.Queries;

public record UserAddressDetailsByIdQuery(Guid AddressId) : IRequest<Result<UserAddressDetails>>;
