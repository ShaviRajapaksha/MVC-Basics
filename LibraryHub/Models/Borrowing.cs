namespace LibraryHub.Models;

public class Borrowing
{
    public int Id { get; set; }
    public DateTime BorrowedDate { get; set; }
    public DateTime? ReturnedDate { get; set; }

    // Foreign Key
    public int BookId { get; set; }

    // Navigation Property
    public Book? Book { get; set; }

    // Foreign Key
    public int MemberId { get; set; }

    // Navigation Property
    public Member? Member { get; set; }
}