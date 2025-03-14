using LibraryMinimalAPI.Context;
using LibraryMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMinimalAPI.Service;

public sealed class BookService: IBookService
{
    private readonly ApplicationDbContext _context;
    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateAsync(Book book, CancellationToken cancellationToken = default)
    {
        await _context.Books.AddAsync(book, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
    public async Task<bool> DeleteAsync(string Isbn, CancellationToken cancellationToken = default)
    {
        Book? book = await _context.Books.FindAsync(Isbn, cancellationToken);
        if(book is null) return false;

        _context.Books.Remove(book);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books.ToListAsync(cancellationToken);
    }

    public async Task<Book?> GetByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
    {
       return await _context.Books.FindAsync(isbn, cancellationToken);
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _context.Books.Where(book => book.Title.Contains(title)).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        _context.Update(book);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}