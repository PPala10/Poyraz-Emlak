using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class ReservationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}