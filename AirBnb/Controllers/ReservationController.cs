using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirBnb.Data;
using AirbnbClone.Data;
using Microsoft.AspNetCore.Authorization;

namespace AirBnb.Controllers
{
    // Controller for Reservation Entity's Page with MVC Protocol
    public class ReservationController : Controller
    {
        private readonly DataContext _context;

        public ReservationController(DataContext context)
        {
            _context = context;
        }

        // Main reservation page (index) view method.
        // The display of reservations on this page changes according to the role of the user viewing the page.
        // If the user is an admin, they can view all reservations.
        // If the user is a host or a guest:
        // they view only the reservations made for their own listing (in the case of a host)
        // or only the reservations they have made themselves (in the case of a guest).
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdClaim, out int currentUserId);

            IQueryable<Reservation> query = _context.Reservations
                .Include(r => r.listing)
                .Include(r => r.guest);

            if (User.IsInRole("Admin"))
            {
            }
            else if (User.IsInRole("Host"))
            {
                query = query.Where(r => r.listing != null && r.listing.hostId == currentUserId);
            }
            else
            {
                query = query.Where(r => r.guestId == currentUserId);
            }

            var reservations = await query.ToListAsync();
            return View(reservations);
        }

        // In booking method system wants some inputs to add reservation to Database.
        // First one variable named duration in a TimeSpan object keeps the reservation duration.
        // And then one variable named nights initialized because of checking invalid date range and overbooking.
        // Also this variable checks the blocked ranges which determined by host.
        // After the checking processes, price_per_night multiplies with duration and service fee added.
        // Reservation saved on a Database with relating attributes, price and status = "Pending"
        // Pending status stayed on Database until payment proceeded.
        [HttpPost]
        public IActionResult Book(int listingId, int guestId, DateTime checkin_date, DateTime checkout_date)
        {
            var currentListing = _context.Listings.FirstOrDefault(l => l.listId == listingId);
            if (currentListing == null)
            {
                return RedirectToAction("Index", "Listing");
            }

            TimeSpan duration = checkout_date.Date - checkin_date.Date;
            int nights = duration.Days;

            if (nights <= 0)
            {
                TempData["ErrorMessage"] = "Geçersiz tarih aralığı seçtiniz.";
                return RedirectToAction("Detail", "Listing", new { id = listingId });
            }

            bool isOverbooking = _context.Reservations.Any(r => 
                r.listId == listingId && r.status != "Cancelled" && r.status != "cancelled" && checkin_date.Date < r.check_out.Date && checkout_date.Date > r.check_in.Date
            );

            if (isOverbooking)
            {
                TempData["ErrorMessage"] = "Seçilen tarih aralığında bu ilan için doluluk mevcuttur.";
                return RedirectToAction("Detail", "Listing", new { id = listingId });
            }

            DateTime utcCheckin = DateTime.SpecifyKind(checkin_date.Date, DateTimeKind.Utc);
            DateTime utcCheckout = DateTime.SpecifyKind(checkout_date.Date, DateTimeKind.Utc);
            
            bool isBlocked = _context.Availabilities.Any(a =>
                a.listId == listingId && a.is_blocked && utcCheckin < a.end_date && utcCheckout > a.start_date);

            if (isBlocked)
            {
                TempData["ErrorMessage"] = "Seçilen tarih aralığı ev sahibi tarafından bloke edilmiştir!";
                return RedirectToAction("Detail", "Listing", new { id = listingId });
            }
            
            decimal serviceFee = 450;
            decimal finalPrice = (nights * currentListing.price_per_night) + serviceFee;

            var newReservation = new Reservation
            {
                listId = listingId,
                guestId = guestId,
                check_in = utcCheckin,
                check_out = utcCheckout,
                total_nights = nights,
                total_price = finalPrice,
                status = "Pending",
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            try
            {
                _context.Reservations.Add(newReservation);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Listing");
            }

            return RedirectToAction("Index");
        }
        
        // System first compares the id which comes from input and Database.
        // If they match, system deletes the reservation from Database.
        // This action can be executed all roles of users.
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.reservationId == id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}