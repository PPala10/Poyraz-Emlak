using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class Availability
{
    [Key]
    public int aid { get; set; } // PK
    public int listId { get; set; } // FK
    public Listing listing { get; set; } // Navigation Prop
    
    public DateTime date { get; set; }
    public bool is_blocked { get; set; }
}