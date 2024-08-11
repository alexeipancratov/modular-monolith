namespace RiverBooks.Users.Contracts.IntegrationEvents;

/// <summary>
/// Signifies that a new user address was added.
/// </summary>
/// <param name="Details">Address details.</param>
/// <remarks>
/// Current listeners: OrderProcessingModule.
/// </remarks>
public record NewUserAddressAddedIntegrationEvent(UserAddressDetails Details) : IntegrationEventBase;
