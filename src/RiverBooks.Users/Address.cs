namespace RiverBooks.Users;

/// <summary>
/// A value type address.
/// NOTE: we didn't want to reuse Address from the Order Processing module since there's a high chance they'll
/// diverge in the near future based on the business requirements.
/// </summary>
/// <remarks>
/// NOTE: we didn't want to reuse Address from the Order Processing module since there's a high chance they'll
/// diverge in the near future based on the business requirements.
/// </remarks>
public record Address(string Street1, string Street2, string City, string State, string PostalCode, string Country);
