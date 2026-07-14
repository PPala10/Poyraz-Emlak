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
        var hosts = _context.Users
            .Where(u => u.role == "Host" || u.role == "host")
            .ToList();

        ViewBag.Hosts = hosts;
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Listing listing, DateTime availabilityStart, DateTime availabilityEnd)
    {
        _context.Listings.Add(listing);
        _context.SaveChanges();

        if (availabilityStart != DateTime.MinValue && availabilityEnd != DateTime.MinValue && availabilityEnd >= availabilityStart)
        {
            _context.Availabilities.Add(new Availability
            {
                listId = listing.listId,
                start_date = DateTime.SpecifyKind(availabilityStart.Date, DateTimeKind.Utc),
                end_date = DateTime.SpecifyKind(availabilityEnd.Date, DateTimeKind.Utc),
                is_blocked = false
            });
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
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
    public IActionResult Edit(Listing updatedListing, DateTime? sync_start, DateTime? sync_end, bool? sync_blocked)
    {
        var existingListing = _context.Listings.FirstOrDefault(l => l.listId == updatedListing.listId);
        if (existingListing == null) return NotFound();

        existingListing.title = updatedListing.title;
        existingListing.description = updatedListing.description;
        existingListing.price_per_night = updatedListing.price_per_night;
        existingListing.city = updatedListing.city;
        existingListing.country = updatedListing.country;
        existingListing.address = updatedListing.address;
        existingListing.property_type = updatedListing.property_type;
        existingListing.room_count = updatedListing.room_count;
        existingListing.bathroom_count = updatedListing.bathroom_count;
        existingListing.max_guests = updatedListing.max_guests;
        existingListing.amenities = updatedListing.amenities;
        existingListing.hostId = updatedListing.hostId;
        existingListing.is_active = updatedListing.is_active;
        existingListing.updated_at = DateTime.UtcNow;

        if (sync_start.HasValue && sync_end.HasValue && sync_end >= sync_start && sync_blocked.HasValue)
        {
            var utcStart = DateTime.SpecifyKind(sync_start.Value.Date, DateTimeKind.Utc);
            var utcEnd = DateTime.SpecifyKind(sync_end.Value.Date, DateTimeKind.Utc);

            var overlapping = _context.Availabilities
                .Where(a => a.listId == updatedListing.listId && a.start_date <= utcEnd && a.end_date >= utcStart)
                .ToList();
            
            _context.Availabilities.RemoveRange(overlapping);

            _context.Availabilities.Add(new Availability
            {
                listId = updatedListing.listId,
                start_date = utcStart,
                end_date = utcEnd,
                is_blocked = sync_blocked.Value
            });
        }

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
