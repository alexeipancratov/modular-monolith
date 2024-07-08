using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.Books;

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
  internal static readonly Guid Book1Guid = new("98999DE8-FBEF-4918-ADFE-DDA0F8B702D9");
  internal static readonly Guid Book2Guid = new("EB9DCAD0-6986-43FF-9A25-11FC79A31448");
  internal static readonly Guid Book3Guid = new("F737D750-4634-492C-BE87-253896EDAF40");
  
  public void Configure(EntityTypeBuilder<Book> builder)
  {
    builder.Property(p => p.Title)
      .HasMaxLength(DataSchemaConstants.DefaultNameLength)
      .IsRequired();
    
    builder.Property(p => p.Author)
      .HasMaxLength(DataSchemaConstants.DefaultNameLength)
      .IsRequired();

    builder.HasData(GetSampleBookData());
  }

  private static IEnumerable<Book> GetSampleBookData()
  {
    const string tolkien = "J.R.R. Tolkien";
    yield return new Book(Book1Guid, "The Fellowship of the Ring", tolkien, 10.99m);
    yield return new Book(Book2Guid, "The Two Towers", tolkien, 11.99m);
    yield return new Book(Book3Guid, "The Return of the King", tolkien, 12.99m);
  }
}
