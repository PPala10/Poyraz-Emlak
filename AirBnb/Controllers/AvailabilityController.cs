using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirBnb.Data;
using AirbnbClone.Data;

namespace AirBnb.Controllers;

public class AvailabilityController : Controller
{
    private readonly DataContext _context;

    public AvailabilityController(DataContext context)
    {
        _context = context;
    }

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