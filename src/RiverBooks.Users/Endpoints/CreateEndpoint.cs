using Ardalis.Result.AspNetCore;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases.User.Create;

namespace RiverBooks.Users.Endpoints;

public record CreateUserRequest(string Email, string Password);

public class CreateEndpoint(IMediator mediator) : Endpoint<CreateUserRequest>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Post("/users");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
  {
    var result = await _mediator.Send(new CreateUserCommand(req.Email, req.Password), ct);

    if (!result.IsSuccess)
    {
      await SendResultAsync(result.ToMinimalApiResult());
      return;
    }

    await SendOkAsync(ct);
  }
}
