using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}