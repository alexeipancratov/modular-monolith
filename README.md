# RiverBooks - A modular monolith sample

It is a demo project of a modular monolith which uses layers. Please note that in a 
production ready project we'd use vertical slices ideally instead (https://deviq.com/practices/vertical-slices).

## Books Module
`dotnet ef migrations add Initial -c BookDbContext -p ../RiverBooks.Books/RiverBooks.Books.csproj -s ./RiverBooks.Web.csproj -o Data/Migrations`

`dotnet ef database update`

To update the testing DB run this command from the Web project folder

`dotnet ef database update -- --environment Testing`

## Users Module

Contains User and Cart Item entities. Please note that the latter could be its own module,
however for this application the decision was made to store it in the same module.

This module uses MediatR as opposed to application service as it is the case in the Books module.

### Create migration

`dotnet ef migrations add CartItems -c UsersDbContext -p ../RiverBooks.Users/RiverBooks.Users.csproj -s ./RiverBooks.Web.csproj -o Data/Migrations`

### Apply migration

`dotnet ef database update -c UsersDbContext`

## Integration between modules
Integration is done using MediatR and a separate Contracts project. Having that projects eliminates a possible
circular dependency between these two modules.

Alternatively, to integrate with the Books module we could've used Materialized View (SQL).