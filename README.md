# SimpleMovies — ASP.NET Core MVC + PostgreSQL

A simple ASP.NET Core MVC project created to understand the basic MVC architecture and how to connect an MVC application to PostgreSQL using Entity Framework Core.

The application allows you to:

* View movies in a table
* Create a new movie
* Edit an existing movie
* Delete a movie
* Store all movie data in PostgreSQL

---

## 1. Technologies Used

* .NET 8
* ASP.NET Core MVC
* C#
* Entity Framework Core
* PostgreSQL
* Npgsql
* Razor Views
* Tailwind CSS (optional, for styling)
* Rider / VS Code

---

## 2. Project Structure

```text
SimpleMovies/
│
├── Controllers/
│   ├── HomeController.cs
│   └── MoviesController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Models/
│   ├── Movie.cs
│   └── ErrorViewModel.cs
│
├── Views/
│   ├── Home/
│   │   └── Index.cshtml
│   │
│   ├── Movies/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   └── Edit.cshtml
│   │
│   └── Shared/
│
├── appsettings.json
├── Program.cs
└── SimpleMovies.csproj
```

---

# 3. Create the MVC Project

Create an ASP.NET Core MVC project.

```bash
dotnet new mvc -n SimpleMovies
cd SimpleMovies
```

Run the project:

```bash
dotnet run
```

---

# 4. Install PostgreSQL EF Core Provider

Install the PostgreSQL provider:

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.11
```

Install Entity Framework Core CLI tools if necessary:

```bash
dotnet tool install --global dotnet-ef
```

Check:

```bash
dotnet ef
```

---

# 5. Create PostgreSQL Database

Connect to PostgreSQL:

```bash
psql -U YOUR_USERNAME -d postgres
```

Create the database:

```sql
CREATE DATABASE "SimpleMoviesDb";
```

Check databases:

```sql
\l
```

Connect to the new database:

```sql
\c SimpleMoviesDb
```

---

# 6. Create the Movie Model

Create:

```text
Models/Movie.cs
```

```csharp
namespace SimpleMovies.Models;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
```

The model represents a movie in our application.

It will eventually represent a database table:

```text
Movies
--------------------------------
Id       integer
Title    text
Genre    text
Price    decimal
```

---

# 7. Create ApplicationDbContext

Create:

```text
Data/ApplicationDbContext.cs
```

```csharp
using Microsoft.EntityFrameworkCore;
using SimpleMovies.Models;

namespace SimpleMovies.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
}
```

The important part is:

```csharp
public DbSet<Movie> Movies { get; set; }
```

This tells Entity Framework Core that the application has a `Movies` entity/table.

---

# 8. Configure the PostgreSQL Connection

Open:

```text
appsettings.json
```

Add the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=SimpleMoviesDb;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "AllowedHosts": "*"
}
```

Replace:

```text
YOUR_USERNAME
YOUR_PASSWORD
```

with your PostgreSQL credentials.

---

# 9. Register DbContext in Program.cs

Open:

```text
Program.cs
```

Add:

```csharp
using Microsoft.EntityFrameworkCore;
using SimpleMovies.Data;
```

Configure the DbContext:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
```

The important part is:

```csharp
options.UseNpgsql(...)
```

This tells Entity Framework Core to use PostgreSQL.

---

# 10. Create the Database Table with EF Core

Create the migration:

```bash
dotnet ef migrations add InitialCreate
```

Apply it:

```bash
dotnet ef database update
```

Now Entity Framework Core creates the `Movies` table in PostgreSQL.

The flow is:

```text
Movie.cs
   ↓
ApplicationDbContext
   ↓
Migration
   ↓
PostgreSQL
   ↓
Movies table
```

---

# 11. Add Test Data

Connect to PostgreSQL:

```bash
psql -U YOUR_USERNAME -d SimpleMoviesDb
```

Insert some movies:

```sql
INSERT INTO "Movies" ("Title", "Genre", "Price")
VALUES
('Inception', 'Sci-Fi', 1200),
('Interstellar', 'Sci-Fi', 1500),
('The Dark Knight', 'Action', 1300),
('Avengers Endgame', 'Action', 1400);
```

Check the data:

```sql
SELECT * FROM "Movies";
```

Example:

```text
 Id |      Title       | Genre  | Price
----+------------------+--------+-------
  1 | Inception        | Sci-Fi | 1200
  2 | Interstellar     | Sci-Fi | 1500
  3 | The Dark Knight  | Action | 1300
  4 | Avengers Endgame | Action | 1400
```

---

# 12. Create MoviesController

Create:

```text
Controllers/MoviesController.cs
```

The controller is responsible for communicating between the View and the database.

```csharp
using Microsoft.AspNetCore.Mvc;
using SimpleMovies.Data;
using SimpleMovies.Models;

namespace SimpleMovies.Controllers;

public class MoviesController : Controller
{
    private readonly ApplicationDbContext _context;

    public MoviesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Movies
    public IActionResult Index()
    {
        var movies = _context.Movies.ToList();

        return View(movies);
    }

    // GET: /Movies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Movies/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        return View(movie);
    }

    // GET: /Movies/Edit/1
    public IActionResult Edit(int id)
    {
        var movie = _context.Movies.Find(id);

        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: /Movies/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        return View(movie);
    }

    // POST: /Movies/Delete/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var movie = _context.Movies.Find(id);

        if (movie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
```

---

# 13. Understand the Controller

## Display movies

```csharp
var movies = _context.Movies.ToList();

return View(movies);
```

This gets the data from PostgreSQL and sends it to the View.

```text
PostgreSQL
    ↓
Movies table
    ↓
_context.Movies
    ↓
ToList()
    ↓
List<Movie>
    ↓
View(movies)
```

---

# 14. Create Movie

The GET action:

```csharp
public IActionResult Create()
{
    return View();
}
```

opens the Create page.

The POST action:

```csharp
[HttpPost]
public IActionResult Create(Movie movie)
{
    if (ModelState.IsValid)
    {
        _context.Movies.Add(movie);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    return View(movie);
}
```

The important part is:

```csharp
_context.Movies.Add(movie);
_context.SaveChanges();
```

This inserts a new record into PostgreSQL.

---

# 15. Edit Movie

Find the movie:

```csharp
var movie = _context.Movies.Find(id);
```

Display it in the Edit View.

After the user changes it:

```csharp
_context.Movies.Update(movie);
_context.SaveChanges();
```

updates the database.

---

# 16. Delete Movie

Find the movie:

```csharp
var movie = _context.Movies.Find(id);
```

Then:

```csharp
_context.Movies.Remove(movie);
_context.SaveChanges();
```

removes it from PostgreSQL.

---

# 17. Create the Movies View

Create:

```text
Views/Movies/Index.cshtml
```

```cshtml
@model IEnumerable<SimpleMovies.Models.Movie>

@{
    ViewData["Title"] = "Movies";
}

<div class="max-w-6xl mx-auto px-6 py-10">

    <div class="flex items-center justify-between mb-8">

        <div>
            <h1 class="text-3xl font-bold">
                Movies
            </h1>

            <p class="text-gray-600 mt-2">
                Movies stored in PostgreSQL
            </p>
        </div>

        <a asp-controller="Movies"
           asp-action="Create"
           class="bg-blue-600 text-white px-5 py-2 rounded-lg">
            + Add Movie
        </a>

    </div>

    <div class="overflow-hidden rounded-lg border bg-white shadow">

        <table class="w-full">

            <thead class="bg-gray-100">

                <tr>
                    <th class="px-6 py-4 text-left">ID</th>
                    <th class="px-6 py-4 text-left">Title</th>
                    <th class="px-6 py-4 text-left">Genre</th>
                    <th class="px-6 py-4 text-left">Price</th>
                    <th class="px-6 py-4 text-left">Actions</th>
                </tr>

            </thead>

            <tbody>

                @foreach (var movie in Model)
                {
                    <tr class="border-t">

                        <td class="px-6 py-4">
                            @movie.Id
                        </td>

                        <td class="px-6 py-4">
                            @movie.Title
                        </td>

                        <td class="px-6 py-4">
                            @movie.Genre
                        </td>

                        <td class="px-6 py-4">
                            Rs. @movie.Price
                        </td>

                        <td class="px-6 py-4">

                            <a asp-action="Edit"
                               asp-route-id="@movie.Id"
                               class="bg-yellow-500 text-white px-3 py-1 rounded">
                                Edit
                            </a>

                            <form asp-action="Delete"
                                  asp-route-id="@movie.Id"
                                  method="post"
                                  class="inline"
                                  onsubmit="return confirm('Are you sure?');">

                                <button type="submit"
                                        class="bg-red-600 text-white px-3 py-1 rounded">
                                    Delete
                                </button>

                            </form>

                        </td>

                    </tr>
                }

            </tbody>

        </table>

    </div>

</div>
```

---

# 18. Understanding `@model`

At the top of the View:

```cshtml
@model IEnumerable<SimpleMovies.Models.Movie>
```

This tells Razor:

> This View expects a collection of Movie objects.

The Controller sends:

```csharp
return View(movies);
```

Therefore:

```text
Controller
    ↓
movies
    ↓
View
    ↓
Model
```

Inside the View:

```cshtml
@foreach (var movie in Model)
```

loops through every movie.

For example:

```cshtml
@movie.Id
@movie.Title
@movie.Genre
@movie.Price
```

accesses the properties of each Movie.

---

# 19. Create View

Create:

```text
Views/Movies/Create.cshtml
```

```cshtml
@model SimpleMovies.Models.Movie

@{
    ViewData["Title"] = "Create Movie";
}

<div class="max-w-xl mx-auto px-6 py-10">

    <h1 class="text-3xl font-bold mb-8">
        Add Movie
    </h1>

    <form asp-action="Create" method="post" class="space-y-5">

        <div>
            <label asp-for="Title"></label>

            <input asp-for="Title"
                   class="w-full border rounded-lg px-4 py-2" />

            <span asp-validation-for="Title"
                  class="text-red-500">
            </span>
        </div>

        <div>
            <label asp-for="Genre"></label>

            <input asp-for="Genre"
                   class="w-full border rounded-lg px-4 py-2" />

            <span asp-validation-for="Genre"
                  class="text-red-500">
            </span>
        </div>

        <div>
            <label asp-for="Price"></label>

            <input asp-for="Price"
                   type="number"
                   step="0.01"
                   class="w-full border rounded-lg px-4 py-2" />

            <span asp-validation-for="Price"
                  class="text-red-500">
            </span>
        </div>

        <button type="submit"
                class="bg-blue-600 text-white px-5 py-2 rounded-lg">
            Create
        </button>

        <a asp-action="Index"
           class="bg-gray-300 px-5 py-2 rounded-lg">
            Cancel
        </a>

    </form>

</div>
```

---

# 20. Edit View

Create:

```text
Views/Movies/Edit.cshtml
```

```cshtml
@model SimpleMovies.Models.Movie

@{
    ViewData["Title"] = "Edit Movie";
}

<div class="max-w-xl mx-auto px-6 py-10">

    <h1 class="text-3xl font-bold mb-8">
        Edit Movie
    </h1>

    <form asp-action="Edit" method="post" class="space-y-5">

        <input type="hidden" asp-for="Id" />

        <div>
            <label asp-for="Title"></label>

            <input asp-for="Title"
                   class="w-full border rounded-lg px-4 py-2" />

            <span asp-validation-for="Title"
                  class="text-red-500">
            </span>
        </div>

        <div>
            <label asp-for="Genre"></label>

            <input asp-for="Genre"
                   class="w-full border rounded-lg px-4 py-2" />

            <span asp-validation-for="Genre"
                  class="text-red-500">
            </span>
        </div>

        <div>
            <label asp-for="Price"></label>

            <input asp-for="Price"
                   type="number"
                   step="0.01"
                   class="w-full border rounded-lg px-4 py-2" />

            <span asp-validation-for="Price"
                  class="text-red-500">
            </span>
        </div>

        <button type="submit"
                class="bg-yellow-500 text-white px-5 py-2 rounded-lg">
            Save Changes
        </button>

        <a asp-action="Index"
           class="bg-gray-300 px-5 py-2 rounded-lg">
            Cancel
        </a>

    </form>

</div>
```

---

# 21. MVC CRUD Flow

The complete application now follows this pattern:

```text
                    PostgreSQL
                         │
                         ▼
                ┌─────────────────┐
                │   EF Core       │
                │ DbContext       │
                └────────┬────────┘
                         │
                         ▼
                MoviesController
                         │
          ┌──────────────┼──────────────┐
          │              │              │
          ▼              ▼              ▼
        Index          Create          Edit
          │              │              │
          ▼              ▼              ▼
     Index.cshtml   Create.cshtml   Edit.cshtml
          │
          ▼
      HTML Table
```

CRUD operations:

```text
CREATE → INSERT
READ   → SELECT
UPDATE → UPDATE
DELETE → DELETE
```

---

# 22. MVC Request Flow

## View Movies

```text
GET /Movies
      ↓
MoviesController.Index()
      ↓
_context.Movies.ToList()
      ↓
PostgreSQL
      ↓
return View(movies)
      ↓
Index.cshtml
      ↓
HTML table
```

## Create Movie

```text
GET /Movies/Create
      ↓
Create.cshtml
      ↓
User fills form
      ↓
POST /Movies/Create
      ↓
MoviesController.Create()
      ↓
_context.Movies.Add(movie)
      ↓
SaveChanges()
      ↓
PostgreSQL
      ↓
Redirect /Movies
```

## Edit Movie

```text
GET /Movies/Edit/1
      ↓
Find movie ID 1
      ↓
Edit.cshtml
      ↓
User changes data
      ↓
POST /Movies/Edit/1
      ↓
_context.Movies.Update(movie)
      ↓
SaveChanges()
      ↓
PostgreSQL
      ↓
Redirect /Movies
```

## Delete Movie

```text
POST /Movies/Delete/1
      ↓
Find movie ID 1
      ↓
_context.Movies.Remove(movie)
      ↓
SaveChanges()
      ↓
PostgreSQL
      ↓
Redirect /Movies
```

---

# 23. Important MVC Concepts Learned

### Model

Represents application data.

```csharp
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public decimal Price { get; set; }
}
```

### View

Displays the data to the user.

```cshtml
@model IEnumerable<Movie>
```

### Controller

Handles requests and communicates with the database.

```csharp
var movies = _context.Movies.ToList();

return View(movies);
```

### DbContext

Connects Entity Framework Core with the database.

```csharp
public DbSet<Movie> Movies { get; set; }
```

### Entity Framework Core

Allows C# code to communicate with PostgreSQL without manually writing SQL for every operation.

---

# 24. Important Razor Concepts

### `@model`

Defines what type of data the View receives.

```cshtml
@model IEnumerable<Movie>
```

### `@foreach`

Loops through the model:

```cshtml
@foreach (var movie in Model)
{
    <p>@movie.Title</p>
}
```

### `@movie.Property`

Accesses a model property:

```cshtml
@movie.Title
@movie.Price
```

### Tag Helpers

Used for MVC links and forms:

```cshtml
asp-action="Edit"
asp-route-id="@movie.Id"
```

For example:

```cshtml
<a asp-action="Edit"
   asp-route-id="@movie.Id">
    Edit
</a>
```

generates a URL such as:

```text
/Movies/Edit/1
```

---

# 25. Useful Commands

Run the application:

```bash
dotnet run
```
```bash
dotnet watch
```
Build:

```bash
dotnet build
```

Install EF Core CLI:

```bash
dotnet tool install --global dotnet-ef
```

Create migration:

```bash
dotnet ef migrations add InitialCreate
```

Apply migration:

```bash
dotnet ef database update
```

List migrations:

```bash
dotnet ef migrations list
```

List installed packages:

```bash
dotnet list package
```

---

# 26. PostgreSQL Commands

Connect:

```bash
psql -U YOUR_USERNAME -d SimpleMoviesDb
```

List databases:

```sql
\l
```

List tables:

```sql
\dt
```

View movies:

```sql
SELECT * FROM "Movies";
```

Exit:

```sql
\q
```

---

# 27. Final Result

The application now has a complete basic CRUD workflow:

```text
                    SimpleMovies
                         │
                         ▼
                    Movies Page
                         │
          ┌──────────────┼──────────────┐
          │              │              │
          ▼              ▼              ▼
       Create           Edit          Delete
          │              │              │
          └──────────────┼──────────────┘
                         ▼
                     PostgreSQL
```

The main lesson is:

```text
Model
  ↓
DbContext
  ↓
Controller
  ↓
View
```

And for CRUD:

```text
Create → Controller → PostgreSQL
Read   → PostgreSQL → Controller → View
Update → Controller → PostgreSQL
Delete → Controller → PostgreSQL
```

This project is a good starting point for understanding ASP.NET Core MVC before moving on to larger applications such as CinemaBooking.


## To add tailwind for Learning/ Testing, add this line in _Layout.cdhtml 
```HTML 
<script src="https://cdn.tailwindcss.com"></script>
``` 
### _Layout.cdhtml 
```HTML 
<!DOCTYPE html> <html lang="en"> 
  <head> <meta charset="utf-8" /> 
    <meta name="viewport" content="width=device-width, initial-scale=1.0" /> 
    <title>@ViewData["Title"] - CinemaBooking</title> 
    <script src="https://cdn.tailwindcss.com"></script> 
  </head> <body> @RenderBody() 
  </body> 
  </html> 
  ```
