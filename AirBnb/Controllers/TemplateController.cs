using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirBnb.Data;
using AirBnb.Models;
using AirbnbClone.Data;
using System.Linq;

namespace AirBnb.Controllers
{
    // Controller for Dashboard Page with MVC Protocol
    public class TemplateController : Controller
    {
        private readonly DataContext _context;

        public TemplateController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new Dashboard();

            model.totalListing = _context.Listings.Count();
            model.totalUser = _context.Users.Count(u => !string.IsNullOrEmpty(u.role) && u.role.Trim().ToLower() == "host");
            model.activeListingNum = _context.Listings.Count(l => l.is_active);
            
            if (model.totalListing > 0)
            {
                model.averageListing = _context.Listings.Average(l => l.price_per_night);
            }

            model.latestListings = _context.Listings
                .Include(l => l.host)
                .OrderByDescending(l => l.created_at)
                .Take(5)
                .ToList();
            
            model.latestsReservations = _context.Reservations
                .Include(r => r.guest)
                .Include(r => r.listing)
                .OrderByDescending(r => r.created_at)
                .Take(5)
                .ToList();

            return View(model);
        }
    }
}