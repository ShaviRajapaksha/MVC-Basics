using LibraryHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryHub.Controllers;

public class MembersController : Controller
{
    private readonly ApplicationDbContext _context;

    public MembersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Members
    public IActionResult Index()
    {
        var members = _context.Members.ToList();

        return View(members);
    }

    // GET: /Members/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Members/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Member member)
    {
        if (ModelState.IsValid)
        {
            _context.Members.Add(member);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }

    // GET: /Members/Edit/1
    public IActionResult Edit(int id)
    {
        var member = _context.Members.Find(id);

        if (member ==null)
        {
            return NotFound();
        }
        return View(member);
    }

    // POST: /Members/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Member member)
    {
        if (id != member.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Members.Update(member);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }

    // GET: /Members/Delete/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var member = _context.Members.Find(id);

        if (member == null)
        {
            return NotFound();
        }

        _context.Members.Remove(member);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}