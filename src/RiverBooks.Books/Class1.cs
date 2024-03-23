﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace RiverBooks.Books;

public static class BooksEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        app.MapGet("/books", (IBookService service) => service.ListBooks());
    }
}

internal interface IBookService
{
    IEnumerable<BookDto> ListBooks();
}

internal class BookService : IBookService
{
    public IEnumerable<BookDto> ListBooks()
    {
        return
        [
            new BookDto(Guid.NewGuid(), "The Fellowship of the Ring", "J.R.R. Tolkien"),
            new BookDto(Guid.NewGuid(), "The Two Towers", "J.R.R. Tolkien"),
            new BookDto(Guid.NewGuid(), "The Return of the King", "J.R.R. Tolkien")
        ];
    }
}

public record BookDto(Guid Id, string Title, string Author);


public static class BookServiceExtensions
{
    public static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        return services;
    }
}