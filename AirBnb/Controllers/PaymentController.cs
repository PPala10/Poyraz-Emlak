using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class PaymentController : Controller
{
    public IActionResult Payment()
    {
        return View();
    }
}