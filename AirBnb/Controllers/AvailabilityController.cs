using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirBnb.Data;
using AirbnbClone.Data;

namespace AirBnb.Controllers;

public class AvailabilityController : Controller
{
    private readonly DataContext _context;

    // Controller for Availability Entity's Page with MVC Protocol
    public AvailabilityController(DataContext context)
    {
        _context = context;
    }

    // Main availability page (index) view method.
    // Any user role can view all the availability ranges for listings, there is no restriction.
    // It shows all open and busy ranges from schedule which determined by hosts and exists reservations.
    public IActionResult Index()
    {
        var listings = _context.Listings
            .Include(l => l.availabilities)
            .Include(l => l.reservations)
            .Where(l => l.is_active)
            .ToList();

        return View(listings);
    }
}