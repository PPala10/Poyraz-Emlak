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
        existingListing.is_active = updatedListing.is_active;
        existingListing.updated_at = DateTime.UtcNow;

        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    
public static string GetMainImage(string propertyType)
{
    var type = propertyType?.ToLower().Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c").Trim();

    if (type == "villa") 
        return "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=500";
    
    if (type == "dag evi" || type == "dag_evi") 
        return "https://images.unsplash.com/photo-1510798831971-661eb04b3739?w=500";
    
    if (type == "studyo") 
        return "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=500";

    return "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500";
}

public static List<string> GetImageGallery(string propertyType)
{
    var type = propertyType?.ToLower().Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c").Trim();

    if (type == "villa")
    {
        return new List<string>
        {
            "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800",
            "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=500",
            "https://images.unsplash.com/photo-1613977257363-707ba9348227?w=500",
            "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=500"
        };
    }
    if (type == "dag evi" || type == "dag_evi")
    {
        return new List<string>
        {
            "https://images.unsplash.com/photo-1510798831971-661eb04b3739?w=800",
            "https://images.unsplash.com/photo-1449034446853-66c86144b0ad?w=500",
            "https://images.unsplash.com/photo-1470770841072-f978cf4d019e?w=500",
            "https://images.unsplash.com/photo-1542718610-a1d656d1884c?w=500"
        };
    }
    if (type == "studyo")
    {
        return new List<string>
        {
            "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=800",
            "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=500",
            "https://images.unsplash.com/photo-1484154218962-a197022b5858?w=500",
            "https://images.unsplash.com/photo-1505691938895-1758d7feb511?w=500"
        };
    }
    return new List<string>
    {
        "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800",
        "https://images.unsplash.com/photo-1560185127-6a2806647f81?w=500",
        "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=500",
        "https://images.unsplash.com/photo-1502672014263-04e8544368fc?w=500"
    };
}
}