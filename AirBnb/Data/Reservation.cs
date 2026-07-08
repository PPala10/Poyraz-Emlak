using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class Reservation
{
    [Key]
    public int reservationId { get; set; }
    
    public int listId { get; set; }
    public int hostId { get; set; }
    public int guestId { get; set; }
    public DateTime check_in { get; set; }
    public DateTime check_out { get; set; }
    public int guest_count { get; set; }
    public decimal total_price { get; set; }
    public bool status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}