using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;


// Database Model for Payment Table
// It has one to one relationship with reservation table
// Payment page shows us to purchased and confirmed reservations  
// In addition that it provides payment infrastructure with using iyzico's sandbox virtual pos machine.
public class Payment
{
    [Key]
    public int paymentId { get; set; } // PK
    public int reservationId { get; set; } // FK
    public Reservation reservation { get; set; } // Navigation Prop
    
    public decimal amount { get; set; }
    public string currency { get; set; }
    public string status { get; set; }
    public string provider { get; set; }
    public string provider_tx_id { get; set; }
    public string provider_response { get; set; }
    public DateTime paid_at { get; set; }
    public DateTime created_at { get; set; }
}

