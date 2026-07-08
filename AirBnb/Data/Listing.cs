using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class Listing 
{
    [Key] public int listId { get; set; }

    public int hostId { get; set; }
    public string? title { get; set; }
    public string? description { get; set; }
    public string? address { get; set; }
    public string? city { get; set; }
    public string? country { get; set; }
    public decimal latitude { get; set; }
    public decimal longitude { get; set; }
    public string? property_type { get; set; }
    public int room_count { get; set; }
    public int bathroom_count { get; set; }
    public int max_guests { get; set; }
    public decimal price { get; set; }
    public string? amenities { get; set; }
    public bool? is_active { get; set; }
    public TimestampAttribute created_at { get; set; }
    public TimestampAttribute updated_at { get; set; }
}

