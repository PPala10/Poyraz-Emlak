using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace AirBnb.Data;

// Database Model for User Table
// It has one to many relationship with listing table
// It has one to many relationship with reservation table
// It has one to many relationship with review table
// There are three user role for this database (Admin, Host, Guest)
// Admin is a main role for controlling, manipulating and organizing the attributes of software.
// Host can create and edit listing details, availability range and view the reviews.
// Guest can book a reservation and make payment about it. Also, he/she can write a review about their booking
// Guest review only activated after the payment succession.
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
