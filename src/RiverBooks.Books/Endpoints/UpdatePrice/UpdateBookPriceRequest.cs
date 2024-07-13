namespace RiverBooks.Books.Endpoints.UpdatePrice;

internal record UpdateBookPriceRequest(Guid Id, decimal NewPrice);
