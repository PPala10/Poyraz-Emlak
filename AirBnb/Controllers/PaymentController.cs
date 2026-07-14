using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class PaymentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}