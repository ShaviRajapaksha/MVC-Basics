using LibraryHub.Data;
using LibraryHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryHub.Controllers;

public class AuthorsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Authors
    public IActionResult Index()
    {
        var authors = _context.Authors.ToList();

        return View(authors);
    }

    // GET: /Authors/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Authors/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        return View(author);
    }

    // GET: /Authors/Edit/1
    public IActionResult Edit(int id)
    {
        var author = _context.Authors.Find(id);

        if (author == null)
        {
            return NotFound();
        }

        return View(author);
    }

    // POST: /Authors/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Author author)
    {
        if (id != author.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        return View(author);
    }

    // POST: /Authors/Delete/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var author = _context.Authors.Find(id);

        if (author == null)
        {
            return NotFound();
        }

        _context.Authors.Remove(author);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}