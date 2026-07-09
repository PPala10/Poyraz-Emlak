using AirBnb.Data;
using AirbnbClone.Data;
using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class ListingController : Controller
{
    private readonly DataContext _context;
    
    public ListingController(DataContext context)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail(int id)
    { 
        return View();
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Listing newList)
    {
        var firstUser = _context.Users.FirstOrDefault();
        if (firstUser != null)
        {
            newList.hostId = firstUser.userId; 
        }
        newList.is_active = true;
        newList.created_at = DateTime.UtcNow;
        newList.updated_at = DateTime.UtcNow;

        _context.Listings.Add(newList);
        _context.SaveChanges();

        return RedirectToAction("Index", "Listing");
    }
}