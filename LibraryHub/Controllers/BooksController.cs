using LibraryHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryHub.Controllers;

public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Books
    public IActionResult Index()
    {
        var books = _context.Books
        .Include(b => b.Author)
        .ToList();
        
        return View(books);
    }

    // GET: /Books/Create
    public IActionResult Create()
    {
        ViewBag.Authors = _context.Authors.ToList();
        return View();
    }

    // POST: /Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        ViewBag.Authors = _context.Authors.ToList();
        return View(book);
    }

    // GET: /Books/Edit/1
    public IActionResult Edit(int id)
    {
        var book = _context.Books.Find(id);

        if (book == null)
        {
            return NotFound();
        }

        ViewBag.Authors = _context.Authors.ToList();
        return View(book);
    }

    // POST: /Books/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Book book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Books.Update(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        ViewBag.Authors = _context.Authors.ToList();
        return View(book);
    }

    // GET: /Books/Delete/1
    public IActionResult Delete(int id)
    {
        var book = _context.Books.Find(id);

        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}