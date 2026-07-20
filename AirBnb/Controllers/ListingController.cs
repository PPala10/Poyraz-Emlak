using AirBnb.Data;
using AirbnbClone.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirBnb.Controllers;

public class ListingController : Controller
{
    private readonly DataContext _context;
    
    // Controller for Listing Entity's Page with MVC Protocol
    public ListingController(DataContext context)
    {
        _context = context;
    }
    
    // Main listing page (index) view method.
    // Every user can view all listings. 
    [Authorize]
    public IActionResult Index()
    {
        var listings = _context.Listings
            .Include(l => l.host)
            .OrderByDescending(l => l.created_at)
            .ToList();
        return View(listings);
    }

    // Detail page view method for each listings with id based implementation.
    // Every user can view all listings. 
    // In detail page guests can create reservations; thus, system pushes the guest list to the view.
    // In order to make reservation user must be guest and verification must be true and listing must be active.
    [HttpGet]
    public IActionResult Detail(int id)
    {
        var listing = _context.Listings.Include(l => l.host)
            .Include(listing => listing.reservations)
            .FirstOrDefault(l => l.listId == id);
        
        ViewBag.guests = _context.Users
            .Where(u => u.role.ToLower().Contains("guest") && u.is_verified == true)
            .ToList();
        
        ViewBag.Reservations = listing?.reservations?
            .Where(r => r.status != "Cancelled" && r.status != "cancelled")
            .ToList();
        
        return View(listing);
    }
    
    // Create page view method.
    // Only admins and hosts can create a listing for their house or buildings.
    // For admins system pushes all hosts to the view in order to select they and assign they to the listing's ownership.
    [HttpGet]
    [Authorize(Roles = "Admin,Host")]
    public IActionResult Create()
    {
        var hosts = _context.Users
            .Where(u => u.role == "Host" || u.role == "host")
            .ToList();

        ViewBag.Hosts = hosts;
        return View();
    }
    
    // Create page form method.
    // System takes all inputs come from frontend selection boxes and creating the new listing object to save it on Database.
    // Hosts can declare some ranges in schedule to blocked area and there is not allowed to book a reservation in that range.
    // This act is not mandatory but hosts can make it. Even if they keep the blocked area empty, system will accept the form.
    [HttpPost]
    [Authorize(Roles = "Admin,Host")]
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

    // Edit page view method.
    // Only admins and hosts can edit a listing for their house or buildings.
    // For admins system pushes all hosts to the view in order to select they and assign they to the listing's ownership.
    // Approximately same dynamics of create form.
    [HttpGet]
    [Authorize(Roles = "Admin,Host")]
    public IActionResult Edit(int id)
    {
        var listing = _context.Listings.FirstOrDefault(l => l.listId == id);
        ViewBag.users = _context.Users.Where(u => u.role.ToLower()
            .Contains("host") && u.is_verified).ToList();
        return View(listing);
    }
    
    // Edit page form method.
    // System takes all inputs come from frontend selection boxes and manipulating the existed listing object to save it on Database.
    // Hosts can declare some ranges in schedule to blocked area and there is not allowed to book a reservation in that range.
    // This act is not mandatory but hosts can make it. Even if they keep the blocked area empty, system will accept the form.
    // At the end of the editing period system again calculates the blocked ranges and updates the availability page and DbSet.
    [HttpPost]
    [Authorize(Roles = "Admin,Host")]
    public IActionResult Edit(Listing updatedListing, DateTime? syncStart, DateTime? syncEnd, bool? syncBlocked)
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

        if (syncStart.HasValue && syncEnd.HasValue && syncEnd >= syncStart && syncBlocked.HasValue)
        {
            var utcStart = DateTime.SpecifyKind(syncStart.Value.Date, DateTimeKind.Utc);
            var utcEnd = DateTime.SpecifyKind(syncEnd.Value.Date, DateTimeKind.Utc);

            var overlapping = _context.Availabilities
                .Where(a => a.listId == updatedListing.listId && a.start_date <= utcEnd && a.end_date >= utcStart)
                .ToList();
            
            _context.Availabilities.RemoveRange(overlapping);

            _context.Availabilities.Add(new Availability
            {
                listId = updatedListing.listId,
                start_date = utcStart,
                end_date = utcEnd,
                is_blocked = syncBlocked.Value
            });
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Delete form mechanism which take listing's id from frontend and delete it from Database.
    // Same as editing, deleting mechanism utilizing id based system, method firstly check the existence of id and compare them. 
    // After the id comparison, method also checks the existence reservations which belong to the listing.
    // If there is any reservation exists, because of the restriction rules system first delete the reservation.
    // After the deletion of reservation, systems deletes the listing.
    // Returns the index page.
    [Authorize(Roles = "Admin,Host")]
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
