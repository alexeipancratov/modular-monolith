# RiverBooks - A modular monolith sample

## Data migration
`dotnet ef migrations add Initial -c BookDbContext -p ../RiverBooks.Books/RiverBooks.Books.csproj -s ./RiverBooks.Web.csproj -o Data/Migrations`

`dotnet ef database update`