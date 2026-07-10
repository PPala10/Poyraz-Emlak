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
        var listing = _context.Listings
            .Include(l => l.host) 
            .Where(l => l.is_active)
            .ToList();
        
        return View(listing);
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
        ViewBag.users = _context.Users.Where(u => u.role != null && u.role.ToLower()
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
        existingListing.hostId = updatedListing.hostId; 
        existingListing.property_type = updatedListing.property_type;
        existingListing.updated_at = DateTime.UtcNow;

        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    
    public static string GetImageByPropertyType(string type)
    {
        return type?.ToLower() switch
        {
            "villa" => "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=500",
            "dağ evi" => "https://images.unsplash.com/photo-1510798831971-661eb04b3739?w=500",
            "stüdyo" => "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=500",
            _ => "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500" // Varsayılan Apartman Dairesi
        };
    }
}