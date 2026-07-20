using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

// Database Model for Reservation Table
// It has one to many relationship with user table
// It has one to many relationship with listing table
// It has one to one relationship with payment table
// It has one to many relationship with review table
// Reservation page shows us to both confirmed and not confirmed (not purchased) reservations. 
// If guest purchases the reservation he/she can gain the review right. 
// If they still do not complete the purchase of reservation, make payment button is activated instead of make review.
public class Reservation
{
    [Key]
    public int reservationId { get; set; } // PK
    public int listId { get; set; } // FK
    public Listing listing { get; set; } // Navigation Prop
    
    public int guestId { get; set; } // FK
    public User guest { get; set; }// Navigation Prop
    
    public DateTime check_in { get; set; }
    public DateTime check_out { get; set; }
    public int guest_count { get; set; }
    public decimal total_price { get; set; }
    public string status { get; set; }
    public int total_nights { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    
    public Payment payment { get; set; }
    public ICollection<Review> reviews { get; set; } = new List<Review>();
}