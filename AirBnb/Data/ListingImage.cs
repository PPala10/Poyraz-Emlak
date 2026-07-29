using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class ListingImage
{
    [Key]
    public int listingImageId  { get; set; }
    public int listingId { get; set; }
    public Listing listing { get; set; }
    public string imagePath  { get; set; }
}
