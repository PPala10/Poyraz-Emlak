using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class ListingImageController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}