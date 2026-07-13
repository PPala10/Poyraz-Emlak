using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class Listing 
{
    [Key]
    public int listId { get; set; } // PK
    public int hostId { get; set; } // FK
    public User host { get; set; }  // Navigation Prop
    
    public string title { get; set; }
    public string description { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public string country { get; set; }
    public string property_type { get; set; }
    public int room_count { get; set; }
    public int bathroom_count { get; set; }
    public int max_guests { get; set; }
    public decimal price_per_night { get; set; }    
    public bool is_active { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public ICollection<Availability> availabilities { get; set; } = new List<Availability>();
    public ICollection<Reservation> reservations { get; set; } = new List<Reservation>();
    public ICollection<Review> reviews { get; set; } = new List<Review>();
}

