using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class ListingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail(int id)
    {
        return View();
    }
    
}