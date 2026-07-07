using System.ComponentModel.DataAnnotations;

namespace AirBnb.Data;

public class users
{
    [Key]
    public int uid { get; set; }
    
    public string? email { get; set; }
    
    public string? password { get; set; }
    
    public string? fname { get; set; }
    
    public string? lname { get; set; }
    
    public string? phone { get; set; }
    
    public string? avatar_url { get; set; }
    
    public string? role { get; set; }
    
    public bool? is_verified { get; set; }
    
    public TimestampAttribute created_at { get; set; }
    
    public TimestampAttribute updated_at { get; set; }
    
}