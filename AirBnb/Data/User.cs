using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace AirBnb.Data;

public class User
{
    [Key]
    public int userId { get; set; } // PK
    
    public string email { get; set; }
    public string password { get; set; }
    public string fname { get; set; }
    public string lname { get; set; }
    public string phone { get; set; }
    public string avatar_url { get; set; }
    public string role { get; set; }
    public bool is_verified { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    
    public ICollection<Listing> hosted_listings { get; set; } = new List<Listing>();
    public ICollection<Reservation> guest_reservations { get; set; } = new List<Reservation>();
    public ICollection<Review> written_reviews { get; set; } = new List<Review>();
}
