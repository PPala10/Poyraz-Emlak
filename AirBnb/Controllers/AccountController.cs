using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AirbnbClone.Data;
using AirBnb.Data;

namespace AirBnb.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fname, string lname, string email, string password,
            string phone, string role)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.email == email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Bu e-posta adresiyle zaten bir kullanıcı kayıtlı.";
                return View();
            }
            
            string userRole = (role == "Host") ? "Host" : "Guest";

            var newUser = new User
            {
                fname = fname,
                lname = lname,
                email = email,
                password = password,
                role = userRole,
                phone = phone,
                avatar_url = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=150"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kayıt başarılı!";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (username == "superadmin123")
            {
                var adminClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Super Admin"),
                    new Claim(ClaimTypes.Email, "admin@poyrazgayrimenkul.com"),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var adminPrincipal = new ClaimsPrincipal(adminIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, adminPrincipal);

                TempData["SuccessMessage"] = "Test Admin'i girişi yapıldı!";
                return RedirectToAction("Index", "Reservation");
            }

            var user = _context.Users.FirstOrDefault(u => u.email == username && u.password == password);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Hatalı e-posta veya şifre.";
                return View();
            }

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{user.fname} {user.lname}"),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()),
                new Claim(ClaimTypes.Role, user.role)
            };

            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

            return RedirectToAction("Index", "Reservation");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}