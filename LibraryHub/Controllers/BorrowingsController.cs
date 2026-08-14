using LibraryHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryHub.Controllers;

public class BorrowingsController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public BorrowingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Borrowings
    public IActionResult Index()
    {
        var borrowings = _context.Borrowings
            .Include(b => b.Book)
            .Include(b => b.Member)
            .ToList();

        return View(borrowings);
    }

    // GET: /Borrowings/Create
    public IActionResult Create()
    {
        ViewBag.Books = _context.Books.ToList();
        ViewBag.Members = _context.Members.ToList();
        return View();
    }

    // POST: /Borrowings/Create
    // POST: /Borrowings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Borrowing borrowing)
    {
        if (ModelState.IsValid)
        {
            // Always store the borrowing time as UTC
            borrowing.BorrowedDate = DateTime.UtcNow;

            // A new borrowing has not been returned yet
            borrowing.ReturnedDate = null;

            _context.Borrowings.Add(borrowing);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Books = _context.Books.ToList();
        ViewBag.Members = _context.Members.ToList();

        return View(borrowing);
    }

    // GET: /Borrowings/Edit/1
    public IActionResult Edit(int id)
    {
        var borrowing = _context.Borrowings.Find(id);

        if(borrowing == null)
        {
            return NotFound();
        }

        ViewBag.Books = _context.Books.ToList();
        ViewBag.Members = _context.Members.ToList();
        return View(borrowing);
    }

    // POST: /Borrowings/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Borrowing borrowing)
    {
        if (id != borrowing.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Make sure DateTimes coming from the form are treated as UTC
            borrowing.BorrowedDate = DateTime.SpecifyKind(
                borrowing.BorrowedDate,
                DateTimeKind.Utc
            );

            if (borrowing.ReturnedDate.HasValue)
            {
                borrowing.ReturnedDate = DateTime.SpecifyKind(
                    borrowing.ReturnedDate.Value,
                    DateTimeKind.Utc
                );
            }

            _context.Borrowings.Update(borrowing);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Books = _context.Books.ToList();
        ViewBag.Members = _context.Members.ToList();

        return View(borrowing);
    }

    // POST: /Borrowings/Delete/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var borrowing = _context.Borrowings.Find(id);

        if ( borrowing == null)
        {
            return NotFound();
        }

        _context.Borrowings.Remove(borrowing);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}