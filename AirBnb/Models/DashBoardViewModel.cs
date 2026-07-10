using AirBnb.Data;

namespace AirBnb.Models;

public class DashBoardViewModel
{
    public int totalListing { get; set; }
    public int totalUser { get; set; }
    public decimal averageListing { get; set; }
    public int activeListingNum { get; set; }
    
    public List<Listing> latestListings { get; set; } = new List<Listing>();
    public List<Reservation> latestsReservations { get; set; } = new List<Reservation>();
}