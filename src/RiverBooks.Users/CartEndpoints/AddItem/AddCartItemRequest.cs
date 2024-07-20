namespace RiverBooks.Users.CartEndpoints.AddItem;

internal record AddCartItemRequest(Guid BookId, int Quantity);
