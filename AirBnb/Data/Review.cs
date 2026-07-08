using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

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