using Microsoft.AspNetCore.Mvc;
using MVC_Basics.Data;
using MVC_Basics.Models;

namespace MVC_Basics.Controllers;
public class MoviesController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public MoviesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Movies
    public IActionResult Index()
    {
        var movies = _context.Movies.ToList();
        return View(movies);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        return View();    
    }

    // POST: Movies/Create
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
    
    // GET: Movies/Edit/1
    public IActionResult Edit(int id)
    {
        var movie = _context.Movies.Find(id);
        if (movie == null)
        {
            return NotFound();
        }
        return View(movie);
    }

    // POST: Movies/Edit/1
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

    // POST: Movies/Delete/1
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


