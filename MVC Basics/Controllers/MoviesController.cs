using Microsoft.AspNetCore.Mvc;
using MVC_Basics.Data;

namespace MVC_Basics.Controllers;
public class MoviesController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public MoviesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var movies = _context.Movies.ToList();
        return View(movies);
    }
}


