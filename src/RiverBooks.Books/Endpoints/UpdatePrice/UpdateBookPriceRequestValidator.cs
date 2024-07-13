using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.Endpoints.UpdatePrice;

internal class UpdateBookPriceRequestValidator : Validator<UpdateBookPriceRequest>
{
  public UpdateBookPriceRequestValidator()
  {
    RuleFor(x => x.Id)
      .NotNull()
      .NotEqual(Guid.Empty)
      .WithMessage("A book ID is required.");

    RuleFor(x => x.NewPrice)
      .GreaterThanOrEqualTo(0)
      .WithMessage("Book price cannot be negative.");
  }
}
