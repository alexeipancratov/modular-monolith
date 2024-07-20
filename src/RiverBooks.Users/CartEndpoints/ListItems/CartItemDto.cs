namespace RiverBooks.Users.CartEndpoints.ListItems;

public record CartItemDto(Guid Id, Guid BookId, string Description, int Quantity, decimal Price);
