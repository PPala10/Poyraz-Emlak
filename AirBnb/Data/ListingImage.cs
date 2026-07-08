using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class ListingImage
{
    [Key]
    public int imageId { get; set; } // PK
    public int listId { get; set; } // FK
    public Listing listing { get; set; } // Navigation Prop
    
    public string image_url { get; set; }
    public bool is_cover { get; set; }
    public int sort_order { get; set; }
    public DateTime updated_at { get; set; }
}