using LibraryHub.Models;

namespace LibraryHub.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Authors.Any())
        {
            return;
        }

        var rowling = new Author
        {
            Name = "J.K. Rowling",
            Country = "United Kingdom"
        };

        var orwell = new Author
        {
            Name = "George Orwell",
            Country = "United Kingdom"
        };

        var martin = new Author
        {
            Name = "George R.R. Martin",
            Country = "United States"
        };

        context.Authors.AddRange(
            rowling,
            orwell,
            martin
        );

        context.SaveChanges();

        var books = new List<Book>
        {
            new Book
            {
                Title = "Harry Potter",
                ISBN = "9780747532699",
                PublishedYear = 1997,
                Price = 2500,
                AuthorId = rowling.Id
            },

            new Book
            {
                Title = "1984",
                ISBN = "9780451524935",
                PublishedYear = 1949,
                Price = 1800,
                AuthorId = orwell.Id
            },

            new Book
            {
                Title = "A Game of Thrones",
                ISBN = "9780553593716",
                PublishedYear = 1996,
                Price = 3000,
                AuthorId = martin.Id
            }
        };

        context.Books.AddRange(books);

        var members = new List<Member>
        {
            new Member
            {
                Name = "John Smith",
                Email = "john@example.com",
                Phone = "0771234567"
            },

            new Member
            {
                Name = "Sarah Wilson",
                Email = "sarah@example.com",
                Phone = "0777654321"
            }
        };

        context.Members.AddRange(members);

        context.SaveChanges();
    }
}