using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Infrastructure;

/// <summary>
/// Materialized view's data model of the address
/// </summary>
internal record OrderAddress(Guid Id, Address Address);
