using AirBnb.Data;
using AirbnbClone.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirBnb.Controllers;

public class ReviewController : Controller
{
    private readonly DataContext _context;
    
    public ReviewController(DataContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var reviews = _context.Reviews
            .Include(r => r.reviewer)
            .Include(r => r.listing)
            .Include(r => r.reservation)
            .OrderByDescending(r => r.created_at)
            .ToList();

        return View(reviews);
    }
    
    [HttpGet]
    public IActionResult Create(int reservationId)
    {
        var reservation = _context.Reservations
            .Include(r => r.listing)
            .Include(r => r.guest)
            .FirstOrDefault(r => r.reservationId == reservationId);

        if (reservation == null)
        {
            TempData["ErrorMessage"] = "Yorum yapılacak rezervasyon kaydı bulunamadı!";
            return RedirectToAction("Index", "Listing");
        }

        var existingReview = _context.Reviews.FirstOrDefault(r => r.reservationId == reservationId);
        if (existingReview != null)
        {
            TempData["ErrorMessage"] = "Bu rezervasyon için daha önce değerlendirme yapılmış!";
            return RedirectToAction("Detail", "Listing", new { id = reservation.listId });
        }

        return View(reservation);
    }
    
    [HttpPost]
    public IActionResult Create(int reservationId, int rating, string comment)
    {
        var reservation = _context.Reservations
            .FirstOrDefault(r => r.reservationId == reservationId);

        if (reservation == null)
        {
            TempData["ErrorMessage"] = "Geçersiz rezervasyon kaydı!";
            return RedirectToAction("Index", "Listing");
        }

        if (string.IsNullOrEmpty(comment) || rating < 1 || rating > 5)
        {
            TempData["ErrorMessage"] = "Lütfen geçerli bir puan ve yorum yazınız!";
            return RedirectToAction("Detail", "Listing", new { id = reservation.listId });
        }

        var newReview = new Review
        {
            reservationId = reservationId,
            reviewerId = reservation.guestId,
            listingId = reservation.listId,
            rating = rating,
            comment = comment,
            created_at = DateTime.UtcNow
        };

        _context.Reviews.Add(newReview);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Değerlendirmeniz başarıyla yayınlandı!";
        return RedirectToAction("Detail", "Listing", new { id = reservation.listId });
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var review = _context.Reviews.FirstOrDefault(r => r.reviewId == id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Yorum başarıyla silindi.";
        }
        return RedirectToAction("Index");
    }
}
