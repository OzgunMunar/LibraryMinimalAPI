using LibraryMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMinimalAPI.Context;

public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options) 
{
    public DbSet<Book> Books { get; set;}
}