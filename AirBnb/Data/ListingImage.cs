using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class ListingImage
{
    [Key]
    public int imageId  { get; set; }
    public int listingId { get; set; }
    public Listing listing { get; set; }
    public string imagePath  { get; set; }
}
