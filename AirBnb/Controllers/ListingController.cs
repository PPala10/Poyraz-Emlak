using AirBnb.Data;
using AirbnbClone.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var listings = _context.Listings
            .Include(l => l.host)
            .OrderByDescending(l => l.created_at)
            .ToList();
        return View(listings);
    }

    [HttpGet]
    public IActionResult Detail(int id)
    {
        var listing = _context.Listings
            .Include(l => l.host)
            .FirstOrDefault(l => l.listId == id);
        
        ViewBag.guests = _context.Users
            .Where(u => u.role != null && u.role.ToLower().Contains("guest"))
            .ToList();
        
        return View(listing);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        var hostUsers = _context.Users
            .Where(u => u.role != null && u.role.ToLower().Contains("host") 
                                       && u.is_verified == true).ToList();
    
        ViewBag.users = hostUsers;
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Listing newList)
    {
        newList.is_active = true;
        newList.created_at = DateTime.UtcNow;
        newList.updated_at = DateTime.UtcNow;

        _context.Listings.Add(newList);
        _context.SaveChanges();

        return RedirectToAction("Index", "Listing");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var listing = _context.Listings.FirstOrDefault(l => l.listId == id);
        ViewBag.users = _context.Users.Where(u => u.role.ToLower()
            .Contains("host") && u.is_verified).ToList();
        return View(listing);
    }
    
    [HttpPost]
    public IActionResult Edit(Listing updatedListing)
    {
        var existingListing = _context.Listings.FirstOrDefault(l => l.listId == updatedListing.listId);

        existingListing.title = updatedListing.title;
        existingListing.description = updatedListing.description;
        existingListing.price_per_night = updatedListing.price_per_night;
        existingListing.city = updatedListing.city;
        existingListing.country = updatedListing.country;
        existingListing.property_type = updatedListing.property_type;
        existingListing.hostId = updatedListing.hostId;
        existingListing.is_active = updatedListing.is_active;
        existingListing.updated_at = DateTime.UtcNow;

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var relatedReservations = _context.Reservations.Where(r => r.listId == id).ToList();
        if (relatedReservations.Any())
        {
            _context.Reservations.RemoveRange(relatedReservations);
        }

        var listing = _context.Listings.FirstOrDefault(l => l.listId == id);
        if (listing != null)
        {
            _context.Listings.Remove(listing);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}
