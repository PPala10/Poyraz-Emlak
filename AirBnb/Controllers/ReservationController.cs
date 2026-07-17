using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirBnb.Data;
using AirbnbClone.Data;

namespace AirBnb.Controllers
{
    public class ReservationController : Controller
    {
        private readonly DataContext _context;

        public ReservationController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var reservations = _context.Reservations
                                       .Include(r => r.guest)
                                       .Include(r => r.listing)
                                       .OrderByDescending(r => r.check_in)
                                       .ToList();

            return View(reservations);
        }

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

            bool isOverlapping = _context.Reservations.Any(r => 
                r.listId == listingId && r.status != "Cancelled" && r.status != "cancelled" && checkin_date.Date < r.check_out.Date && checkout_date.Date > r.check_in.Date
            );

            if (isOverlapping)
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