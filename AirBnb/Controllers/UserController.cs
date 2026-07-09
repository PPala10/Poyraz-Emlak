using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class UserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail(int id)
    {
        return View();
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    public IActionResult Edit(int id)
    {
        return View();
    }
}