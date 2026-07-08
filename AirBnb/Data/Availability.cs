using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class Availability
{
    [Key]
    public int aid { get; set; }
    
    public int listId { get; set; }
    public DateTime date { get; set; }
    public bool is_blocked { get; set; }
}