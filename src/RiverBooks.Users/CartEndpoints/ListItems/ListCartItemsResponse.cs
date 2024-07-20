namespace RiverBooks.Users.CartEndpoints.ListItems;

public class ListCartItemsResponse
{
  public IReadOnlyCollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
}
