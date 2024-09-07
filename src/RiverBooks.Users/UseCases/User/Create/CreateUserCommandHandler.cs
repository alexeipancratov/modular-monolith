using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RiverBooks.EmailSending.Contracts;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.UseCases.User.Create;

internal class CreateUserCommandHandler(
  UserManager<ApplicationUser> userManager,
  IMediator mediator)
  : IRequestHandler<CreateUserCommand, Result>
{
  private readonly UserManager<ApplicationUser> _userManager = userManager;
  private readonly IMediator _mediator = mediator;

  public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {
    var newUser = new ApplicationUser { Email = request.Email, UserName = request.Email };

    var result = await _userManager.CreateAsync(newUser, request.Password);

    if (!result.Succeeded)
    {
      return Result.Error(new ErrorList(result.Errors.Select(x => x.Description).ToArray()));
    }

    var sendEmailCommand = new SendEmailCommand(
      To: request.Email,
      From: "donotreply@riverbooks.com",
      Subject: "Welcome to River Books",
      Body: "Thank you for registering with River Books");
    await _mediator.Send(sendEmailCommand, cancellationToken);

    return Result.Success();
  }
}
