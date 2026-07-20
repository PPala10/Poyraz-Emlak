using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

// Database Model for Availability Table
// It has one to many relationship with listing table
// Availability page shows us to available ranges that determined by host and days which can be booked.
public class Availability
{
    [Key]
    public int aid { get; set; } // PK
    public int listId { get; set; } // FK
    public Listing listing { get; set; } // Navigation Prop
    
    public DateTime start_date { get; set; }
    public DateTime end_date { get; set; }
    public bool is_blocked { get; set; }
}