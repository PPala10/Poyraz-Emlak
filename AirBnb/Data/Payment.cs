using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class Payment
{
    [Key]
    public int paymentId { get; set; }
    
    public int reservationId { get; set; }
    public decimal amount { get; set; }
    public string currency { get; set; }
    public bool status { get; set; }
    public string provider { get; set; }
    public string provider_tx_id { get; set; }
    public JsonContent provider_response { get; set; }
    public DateTime paid_at { get; set; }
    public DateTime created_at { get; set; }
}

