# RiverBooks - A modular monolith sample

It is a demo project of a modular monolith which uses layers. Please note that in a 
production ready project we'd use vertical slices ideally instead (https://deviq.com/practices/vertical-slices).

## Data migration
`dotnet ef migrations add Initial -c BookDbContext -p ../RiverBooks.Books/RiverBooks.Books.csproj -s ./RiverBooks.Web.csproj -o Data/Migrations`

`dotnet ef database update`

To update the testing DB run this command from the Web project folder

`dotnet ef database update -- --environment Testing`

## Users Module

Create migration

`dotnet ef migrations add CartItems -c UsersDbContext -p ../RiverBooks.Users/RiverBooks.Users.csproj -s ./RiverBooks.Web.csproj -o Data/Migrations`

Apply migration

`dotnet ef database update -c UsersDbContext`