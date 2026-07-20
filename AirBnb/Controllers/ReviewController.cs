using AirBnb.Data;
using AirbnbClone.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirBnb.Controllers;

public class ReviewController : Controller
{
    private readonly DataContext _context;
    
    // Controller for Review Entity's Page with MVC Protocol
    public ReviewController(DataContext context)
    {
        _context = context;
    }
    
    // Main review page (index) view method.
    // Any user role can view all the reviews, there is no restriction.
    // Page shows all reviews with guest which creates the review, rating and review itself.
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
    
    // Create page for reviews.
    // Each user only create review for their reservation and each reservation has only one review from one user.
    // System first check user's reservations by id based standards.
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
    
    // Create form for generating review.
    // User gives the input of rating between 1 and 5 and comment for review.
    // Review created with using user inputs and system added it to the Database.
    // It returns the detail page of listing which published review about it.
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

    // Only admin can delete published reviews.
    [HttpPost]
    [Authorize(Roles = "Admin")]
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
