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
            DateTime utcCheckin = DateTime.SpecifyKind(checkin_date, DateTimeKind.Utc);
            DateTime utcCheckout = DateTime.SpecifyKind(checkout_date, DateTimeKind.Utc);

            var currentListing = _context.Listings.FirstOrDefault(l => l.listId == listingId);
            if (currentListing == null)
            {
                return RedirectToAction("Index", "Listing");
            }

            TimeSpan duration = utcCheckout - utcCheckin;
            int nights = duration.Days;

            if (nights <= 0)
            {
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
                status = "Confirmed",
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
    }
}