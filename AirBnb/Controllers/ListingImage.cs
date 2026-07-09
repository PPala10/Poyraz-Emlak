using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class ListingImage : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}