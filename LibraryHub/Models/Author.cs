namespace LibraryHub.Models;

    public class Author
    {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Relationships
    public ICollection<Book> Books { get; set; } = new List<Book>();
    }
