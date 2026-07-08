using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

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
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    
    public Payment payment { get; set; }
    public ICollection<Review> reviews { get; set; } = new List<Review>();
}