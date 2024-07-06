using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RiverBooks.Books.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Books");

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Books",
                table: "Books",
                columns: new[] { "Id", "Author", "Price", "Title" },
                values: new object[,]
                {
                    { new Guid("98999de8-fbef-4918-adfe-dda0f8b702d9"), "J.R.R. Tolkien", 10.99m, "The Fellowship of the Ring" },
                    { new Guid("eb9dcad0-6986-43ff-9a25-11fc79a31448"), "J.R.R. Tolkien", 11.99m, "The Two Towers" },
                    { new Guid("f737d750-4634-492c-be87-253896edaf40"), "J.R.R. Tolkien", 12.99m, "The Return of the King" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books",
                schema: "Books");
        }
    }
}
