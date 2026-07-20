using AirBnb.Data;
using AirbnbClone.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

// Controller for User Entity's Page with MVC Protocol
public class UserController : Controller
{
    private readonly DataContext _context;
    
    public UserController(DataContext context)
    {
        _context = context;
    }
    
    // Only users which their role are admin can view all users and users page.
    // Because of every page which is related to the user interface placed inside the user page,
    // Only admins can access the user interface.
    
    
    // Main user page (index) view method.
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var allUsers = _context.Users.ToList();
        return View(allUsers);
    }
   
    // Register page for adding new users in Database.
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    // Register form mechanism which take the user from frontend in a user object and save it in Database
    // If admin forgets to select avatar for a user, system assigns default avatar image.
    // Returns the index page.
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

        return RedirectToAction("Index", "User");
    }
    
    // Edit page for editing users' attributes in Database.
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.userId == id);
        if (user == null) return NotFound();

        return View(user);
    }
    
    // Edit form mechanism which take the updated user from frontend in a user object and save it in Database.
    // Because of editing mechanism utilizing id based system, method firstly check the existence of id and compare them. 
    // Any attribute in a user entity can be manipulated in Edit form.
    // Returns the index page.
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
    
    // Delete form mechanism which take user's id from frontend and delete it from Database.
    // Same as editing, deleting mechanism utilizing id based system, method firstly check the existence of id and compare them. 
    // After the id comparison, method also checks the existence listings which belong to the user.
    // If there is any listing exists, because of the restriction rules system first delete the listings.
    // After the deletion of listings, systems deletes the user.
    // Returns the index page.
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