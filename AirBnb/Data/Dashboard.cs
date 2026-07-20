using AirBnb.Data;

namespace AirBnb.Models;

// Model for Dashboard View
// Dashboard page shows us to totalListings and totalUsers numbers.
// In addition that, it shows 5 latestListings and 5 latestReservations.
// Lastly averageListing amount and activeListingNum which decided by host.
public class Dashboard
{
    public int totalListing { get; set; }
    public int totalUser { get; set; }
    public decimal averageListing { get; set; }
    public int activeListingNum { get; set; }
    
    public List<Listing> latestListings { get; set; } = new List<Listing>();
    public List<Reservation> latestsReservations { get; set; } = new List<Reservation>();
}