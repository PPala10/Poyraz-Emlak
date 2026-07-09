using AirBnb.Data;
using AirbnbClone.Data;
using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class UserController : Controller
{
    private readonly DataContext _context;
    
    public UserController(DataContext context)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail(int id)
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Register(User newUser)
    {

        newUser.role = "host"; 
        newUser.is_verified = true;
        newUser.avatar_url = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=100"; // Sahte bir profil resmi
        newUser.created_at = DateTime.UtcNow;
        newUser.updated_at = DateTime.UtcNow;

        _context.Users.Add(newUser); 
        _context.SaveChanges(); 

        return RedirectToAction("Create", "Listing");
    }
    
    public IActionResult Edit(int id)
    {
        return View();
    }
}