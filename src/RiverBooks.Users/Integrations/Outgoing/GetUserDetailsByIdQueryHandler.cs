using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.Contracts.Queries;
using RiverBooks.Users.UseCases.User.GetById;

namespace RiverBooks.Users.Integrations.Outgoing;

// NOTE: This command handler delegates the work to an internal use-case, because only those classes
// should be allowed to work with domain types (ideally).
// In other words, it's better to have integration handlers to work with Use Cases, just like the endpoints would.
internal class GetUserDetailsByIdQueryHandler(IMediator mediator) : IRequestHandler<GetUserDetailsByIdQuery, Result<UserDetails>>
{
  private readonly IMediator _mediator = mediator;

  public async Task<Result<UserDetails>> Handle(GetUserDetailsByIdQuery request, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new GetUserByIdQuery(request.UserId), cancellationToken);

    return result.Map(u => new UserDetails(u.Id, u.EmailAddress));
  }
}
