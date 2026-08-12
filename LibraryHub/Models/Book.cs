namespace LibraryHub.Models;

    public class Book
    {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public decimal Price { get; set; }

    // Foreign Key
    public int AuthorId { get; set; }

    // Navigation Property
    public Author? Author { get; set; } = null!;

    // Relationships
    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
    }
