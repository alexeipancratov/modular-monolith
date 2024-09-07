using Ardalis.Result;
using MediatR;

namespace RiverBooks.Users.Contracts.Queries;

public record GetUserDetailsByIdQuery(Guid UserId) : IRequest<Result<UserDetails>>;
