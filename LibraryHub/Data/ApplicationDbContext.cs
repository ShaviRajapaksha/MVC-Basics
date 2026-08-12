using Microsoft.EntityFrameworkCore;

namespace LibraryHub.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<Borrowing> Borrowings { get; set; } = null!;
}