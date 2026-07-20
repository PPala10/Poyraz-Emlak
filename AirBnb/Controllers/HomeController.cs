using AirBnb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AirBnb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult SendMessage(string fullName, string email, string subject, string message)
        {
            TempData["SuccessMessage"] = "Mesajınız başarıyla sunucularımıza iletildi!";
            return RedirectToAction("Contact");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Error { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
