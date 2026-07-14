using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirBnb.Data;

public class Listing 
{
    [Key]
    public int listId { get; set; } // PK
    public int hostId { get; set; } // FK
    public User host { get; set; }  // Navigation Prop
    
    public string title { get; set; }
    public string description { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public string country { get; set; }
    public string property_type { get; set; }
    public string amenities { get; set; }
    public int room_count { get; set; }
    public int bathroom_count { get; set; }
    public int max_guests { get; set; }
    public decimal price_per_night { get; set; }    
    public bool is_active { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    
    [NotMapped]
    public string MainImage => ImageGallery.FirstOrDefault() ?? "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800";

    [NotMapped]
    public List<string> ImageGallery
    {
        get
        {
            var type = property_type?.ToLower() ?? "";

            if (type.Contains("villa"))
            {
                return new List<string>
                {
                    "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800",
                    "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=500",
                    "https://images.unsplash.com/photo-1613977257363-707ba9348227?w=500",
                    "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=500"
                };
            }
        
            if (type.Contains("dag") || type.Contains("dağ") || type.Contains("cabin"))
            {   
                return new List<string>
                {
                    "https://images.unsplash.com/photo-1510798831971-661eb04b3739?w=800",
                "https://images.unsplash.com/photo-1449034446853-66c86144b0ad?w=500",
                "https://images.unsplash.com/photo-1470770841072-f978cf4d019e?w=500",
                "https://images.unsplash.com/photo-1542718610-a1d656d1884c?w=500"
            };
            }
            
            if (type.Contains("studyo") || type.Contains("stüdyo") || type.Contains("studio"))
            {
                return new List<string>
                {
                    "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=800",
                    "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=500",
                    "https://images.unsplash.com/photo-1484154218962-a197022b5858?w=500",
                    "https://images.unsplash.com/photo-1505691938895-1758d7feb511?w=500"
                };
            }
            
            if (type.Contains("tas") || type.Contains("taş") || type.Contains("stone"))
            {
                return new List<string>
                {
                    "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
                    "https://images.unsplash.com/photo-1502005229762-fc1b2b812ca5?w=500",
                    "https://images.unsplash.com/photo-1600210492486-724fe5c67fb0?w=500",
                    "https://images.unsplash.com/photo-1598228723793-52759bba239c?w=500"
                };
            }
            
            if (type.Contains("apartman") || type.Contains("daire") || type.Contains("flat"))
            {
                return new List<string>
                {
                    "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800",
                    "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=500",
                    "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500",
                    "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=500"
                };
            }

            int remainder = listId % 3;
            if (remainder == 0)
            {
                return new List<string>
                {
                    "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800",
                    "https://images.unsplash.com/photo-1560185127-6a2806647f81?w=500",
                    "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=500",
                    "https://images.unsplash.com/photo-1502672014263-04e8544368fc?w=500"
                };
            }
            if (remainder == 1)
            {
                return new List<string>
                {
                    "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800",
                    "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=500",
                    "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500",
                    "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=500"
                };
            }
            return new List<string>
            {
                "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
                "https://images.unsplash.com/photo-1502005229762-fc1b2b812ca5?w=500",
                "https://images.unsplash.com/photo-1600210492486-724fe5c67fb0?w=500",
                "https://images.unsplash.com/photo-1598228723793-52759bba239c?w=500"
            };
        }
    }
    public ICollection<Availability> availabilities { get; set; } = new List<Availability>();
    public ICollection<Reservation> reservations { get; set; } = new List<Reservation>();
    public ICollection<Review> reviews { get; set; } = new List<Review>();
}

