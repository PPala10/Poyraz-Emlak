using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class ReviewController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}