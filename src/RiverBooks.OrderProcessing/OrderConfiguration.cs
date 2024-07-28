using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.OrderProcessing;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  void IEntityTypeConfiguration<Order>.Configure(EntityTypeBuilder<Order> builder)
  {
    builder.Property(o => o.Id)
      .ValueGeneratedNever();

    builder.ComplexProperty(o => o.ShippingAddress, address =>
    {
      address.Property(a => a.Street1)
        .HasMaxLength(Constants.StreetMaxLength);
      address.Property(a => a.Street2)
        .HasMaxLength(Constants.StreetMaxLength);
      address.Property(a => a.City)
        .HasMaxLength(Constants.CityMaxLength);
      address.Property(a => a.State)
        .HasMaxLength(Constants.StateMaxLength);
      address.Property(a => a.PostalCode)
        .HasMaxLength(Constants.PostalcodeMaxLength);
      address.Property(a => a.Country)
        .HasMaxLength(Constants.CountryMaxLength);
    });

    builder.ComplexProperty(o => o.BillingAddress, address =>
    {
      address.Property(a => a.Street1)
        .HasMaxLength(Constants.StreetMaxLength);
      address.Property(a => a.Street2)
        .HasMaxLength(Constants.StreetMaxLength);
      address.Property(a => a.City)
        .HasMaxLength(Constants.CityMaxLength);
      address.Property(a => a.State)
        .HasMaxLength(Constants.StateMaxLength);
      address.Property(a => a.PostalCode)
        .HasMaxLength(Constants.PostalcodeMaxLength);
      address.Property(a => a.Country)
        .HasMaxLength(Constants.CountryMaxLength);
    });
  }
}
