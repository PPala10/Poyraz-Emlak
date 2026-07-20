using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

// Database Model for Review Table
// It has one to one relationship with reservaition table
// It has one to many relationship with user table
// It has one to many relationship with listing table
// Every user can create one review for each reservation.
// Reviews can be controlled and manipulated by admins 
// Reviews can be monitoring by every user role.
public class Review
{
    [Key]
    public int reviewId { get; set; } // PK
    
    public int reservationId { get; set; } // FK
    public Reservation reservation { get; set; } // Navigation Prop
    
    public int reviewerId { get; set; } // FK
    public User reviewer { get; set; } // Navigation Prop
    
    public int listingId { get; set; } // FK 
    public Listing listing { get; set; } // Navigation Prop
    
    public int rating { get; set; }
    public string comment  { get; set; }
    public DateTime created_at { get; set; }
}