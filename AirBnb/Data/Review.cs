using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class Review
{
    [Key]
    public int reviewId { get; set; }
    
    public int reservationId { get; set; }
    public int reviewerId { get; set; }
    public int listingId { get; set; }
    public decimal rating { get; set; }
    public string comment  { get; set; }
    public TimestampAttribute created_at { get; set; }
}