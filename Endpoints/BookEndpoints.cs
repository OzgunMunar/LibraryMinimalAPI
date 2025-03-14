using FluentValidation.Results;
using LibraryMinimalAPI.Models;
using LibraryMinimalAPI.Service;
using LibraryMinimalAPI.Validator;
using Microsoft.AspNetCore.Authorization;

namespace LibraryMinimalAPI.Endpoints;

public static class BookEndpoints
{
    public static void UseBookEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapGet("login", (JwtProvider JwtProvider) => {

            return Results.Ok(new { Token = JwtProvider.CreateToken() });

        })
        .WithTags("Auth Endpoints");

        app.MapPost("create-book", async (Book book, IBookService bookService, CancellationToken cancellationToken) => {
            
            BookValidator bookValidator = new();
            ValidationResult validationResult = bookValidator.Validate(book);

            if(!validationResult.IsValid) 
            {
                return Results.BadRequest(validationResult.Errors.Select(s => s.ErrorMessage));
            }

            var result = await bookService.CreateAsync(book, cancellationToken);
            if(!result) return Results.BadRequest("Something went wrong!");

            return Results.CreatedAtRoute("SearchBookByIsbn", new { IsbnNo = book.Isbn });
            // return Results.Created($"/books/{book.Isbn}", book);
            // return Results.Ok(new { Message = "Book created." });

        })
        .WithName("CreateBook")
        .WithTags("Book Endpoints");

        // We don't want this endpoint to work if user is not authorized.
        app.MapGet("get-all-books", [Authorize] async (IBookService bookService, CancellationToken cancellationToken) => {
        
            IEnumerable<Book> books = await bookService.GetAllAsync(cancellationToken);
            return Results.Ok(books);

        })
        .WithName("GetAllBooks")
        .WithTags("Book Endpoints");

        app.MapGet("searchbookbyisbn/{IsbnNo}", async (string IsbnNo, IBookService bookService, CancellationToken cancellationToken) => {

            Book? book = await bookService.GetByIsbnAsync(IsbnNo, cancellationToken);
            return book is null ? Results.NotFound() : Results.Ok(book);

        })
        .WithName("SearchBookByIsbn")
        .WithTags("Book Endpoints");

        app.MapGet("searchbooksbytitle/{title}", async (string title, IBookService bookService, CancellationToken cancellationToken) => {

            IEnumerable<Book> books = await bookService.SearchByTitleAsync(title, cancellationToken);
            return books is null ? Results.NotFound(cancellationToken) : Results.Ok(books);

        })
        .WithName("SearchBooksByTitle")
        .WithTags("Book Endpoints");

        app.MapPut("update-book", async(Book book, IBookService bookService, CancellationToken cancellationToken) => {

            BookValidator bookValidator = new();
            ValidationResult validationResult = bookValidator.Validate(book);

            if(!validationResult.IsValid) {
                return Results.BadRequest(validationResult.Errors.Select(s => s.ErrorMessage));
            }

            var result = await bookService.UpdateAsync(book, cancellationToken);
            if(!result) return Results.BadRequest("Something went wrong");

            return Results.Ok(new { Message = "Book update is successful." });

        })
        .WithName("UpdateBook")
        .WithTags("Book Endpoints");

        app.MapDelete("delete-book/{isbn}", async (string isbn, IBookService bookService, CancellationToken cancellationToken) => {

            bool result = await bookService.DeleteAsync(isbn, cancellationToken);
            return result is true ? Results.Ok(true) : Results.Problem("There is been a problem during deletion.");

        })
        .WithName("DeleteBook")
        .WithTags("Book Endpoints");
        
    }

}