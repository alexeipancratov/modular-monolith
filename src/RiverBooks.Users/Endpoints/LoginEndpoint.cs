using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Users.Domain;

namespace RiverBooks.Users.Endpoints;

internal record UserLoginRequest(string Email, string Password);

internal class LoginEndpoint(UserManager<ApplicationUser> userManager) : Endpoint<UserLoginRequest>
{
  private readonly UserManager<ApplicationUser> _userManager = userManager;

  public override void Configure()
  {
    Post("/users/login");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UserLoginRequest req, CancellationToken ct)
  {
    var user = await _userManager.FindByEmailAsync(req.Email);
    if (user == null)
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    var loginSuccessful = await _userManager.CheckPasswordAsync(user, req.Password);

    if (!loginSuccessful)
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    var jwtSecret = Config["Auth:JwtSecret"];
    var token = JwtBearer.CreateToken(opt =>
    {
      opt.User.Claims.Add(new Claim("EmailAddress", user.Email!));
      opt.SigningKey = jwtSecret!;
    });

    await SendAsync(token, 200, ct);
  }
}
