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
        var allUsers = _context.Users.ToList();
        return View(allUsers);
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
        newUser.is_verified = true;
        if (string.IsNullOrEmpty(newUser.avatar_url))
        {
            newUser.avatar_url = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=150";
        }        
        newUser.created_at = DateTime.UtcNow;
        newUser.updated_at = DateTime.UtcNow;

        _context.Users.Add(newUser); 
        _context.SaveChanges(); 

        return RedirectToAction("Create", "Listing");
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.userId == id);
        if (user == null) return NotFound();

        return View(user);
    }
    
    [HttpPost]
    public IActionResult Edit(User updatedUser)
    {
        
        var user = _context.Users.FirstOrDefault(u => u.userId == updatedUser.userId);
        if (user == null) return NotFound();

        user.fname = updatedUser.fname;
        user.lname = updatedUser.lname;
        user.email = updatedUser.email;
        user.phone = updatedUser.phone;
        user.updated_at = DateTime.UtcNow;
        user.role = updatedUser.role;              
        user.is_verified = updatedUser.is_verified; 
        user.avatar_url = updatedUser.avatar_url;   
        user.updated_at = DateTime.UtcNow;

        _context.SaveChanges(); 
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.userId == id);
        if (user == null) return NotFound();

        var userList = _context.Listings.Where(l => l.hostId == id).ToList();
        if (userList.Any())
        {
            _context.Listings.RemoveRange(userList);
        }

        _context.Users.Remove(user);
        _context.SaveChanges(); 

        return RedirectToAction("Index");
    }
}