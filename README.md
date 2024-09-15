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

## Order Processing Module

### Create and apply migration

`dotnet ef migrations add Initial_OrderProcessing -c OrderProcessingDbContext -p ../RiverBooks.OrderProcessing/RiverBooks.OrderProcessing.csproj -s ./RiverBooks.Web.csproj -o Data/Migrations`

`dotnet ef database update -c OrderProcessingDbContext`

## Integration between modules
Integration is done using MediatR and a separate Contracts project. Having that projects eliminates a possible
circular dependency between two modules.

Alternatively, to integrate with the Books module we could've used Materialized View (SQL).

## Materialized View - Order Processing module
An actual implementation of the Materialized View pattern is implemented for the Order Addresses functionality.
User addresses are ultimately being stored in the Users module, but Order Processing module needs them too.
In order to avoid fetching addresses every time Order Processing module implements the materialized view pattern
where it maintains a Redis cache copy of the user addresses which are being updated based on the
`NewUserAddressAddedIntegrationEvent` which is being emitted by the Users module.

### Testing
In order to test this functionality:
1. add a new address to the user
2. add an item to the cart
3. checkout and indicate the ID of the newly added address as both Billing/Shipping

## Clean Architecture
Users and OrderProcessing modules in this solution follow the Clean Architecture pattern,
although we're not breaking down each module into separate projects which would normally enforce
the Clean Architecture rules (e.g., Clean Architecture solution pattern).
This is the usual approach in modular monoliths. However, in order to continue enforcing these
principles we're using ArchUnit tests.
NOTE: We're not separating logic into several assemblies in Modular Monolith because
we want to be able to enforce access using the `internal` keyword. (review this again later)

## Validation
In the Users module we're using the FluentValidation Nuget package. The validation is configured
as a MediatR behavior which leverages the FluentValidation validators. Therefore, we follow the
Chain of Responsibility pattern. The validation is implemented at the MediatR command level
as opposed to Endpoint level in order to be able to more easily switch to another ASP.NET framework
in the future (and to avoid breaking changes which could be done in the FastEndpoints).

## EmailSending module
This module relies on an SMTP server to be running.
E.g., `docker run --name=papercut -p 25:25 -p 37408:37408 jijiechen/papercut:latest -d`.
And also on MongoDB server - `docker run --name mongodb -d -p 27017:27017 mongo`.

It follows (at least partially) the Vertical Slice architecture.

NOTE: each module can follow different patterns internally (i.e., Vertical Slice architecture, Clean Architecture, etc.)
while the whole application is following the Modular Monolith architecture.

### Outbox pattern
Allows to send emails at a later point, thus making this process more reliable and non-blocking.

NOTE: We could've implemented it directly in the Order Service so that it gets persisted as part of a SQL transaction.
But that would be an overkill in our example. Therefore, we opted for persisting outbox in Email Service's
own DB, which is MongoDB.

We have a background service which retrieves undelivered emails and tries to send them, every 10 seconds.

## Runtime dependencies
This project requires the following dependencies to be up and running:
- SQL DB (for data)
- Redis (for caching data)
- SMTP server (e.g., "Papercut", with default settings)
- MongoDB (for emails - Outbox pattern)

## Outstanding issues
- Cart isn't being cleared after checkout