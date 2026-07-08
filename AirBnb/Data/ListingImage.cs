using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class ListingImage
{
    [Key]
    public int imageId { get; set; }
    
    public int listId { get; set; }
    public string? image_url { get; set; }
    public bool is_cover { get; set; }
    public int sort_order { get; set; }
    public TimestampAttribute updated_at { get; set; }
}