namespace RiverBooks.Books.Endpoints.UpdateBookPrice;

internal record UpdateBookPriceRequest(Guid Id, decimal NewPrice);
