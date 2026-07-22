using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AirbnbClone.Data;
using AirBnb.Data;
using Microsoft.AspNetCore.Identity;

namespace AirBnb.Controllers
{
    // Controller for Login and Register Pages with MVC Protocol
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }
        
        // Helper method in order to hash already existed passwords in Database
        // Thanks to this method all password in Database encrypted and stored.
        [HttpGet]
        public async Task<IActionResult> MigratePasswords()
        {
            var users = _context.Users.ToList();
            var passwordHasher = new PasswordHasher<User>();
            int updatedCount = 0;

            foreach (var user in users)
            {
                if (string.IsNullOrEmpty(user.password) || !user.password.StartsWith("AQAAAA"))
                {
                    user.password = passwordHasher.HashPassword(user, user.password);
                    updatedCount++;
                }
            }
            
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"{updatedCount} adet kullanıcının şifresi başarıyla hash'lendi!";
            return RedirectToAction("Login");
        }

        // Main register page (index) view method.
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Register form mechanism which take the user's attributes from frontend and save it in Database
        // Firstly system checks existing users using email base comparison. If there is an email match, 
        // Systems returns error with message and do not allow the registration.
        // In this register page admin registration cannot be possible. 
        // Only another admin or superadmin (will be mentioned in login method) can add another admin
        // Or change the user's role to an admin.
        // Passwords are encrypted by EfCore's hash functions and stored in database with hash versions
        // Returns the login page if registration be successful and save the user in Database.
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
                avatar_url = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=150",
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            
            var locker = new PasswordHasher<User>();
            newUser.password = locker.HashPassword(newUser, password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Kayıt başarılı!";
            return RedirectToAction("Login");
        }

        // Main register page (index) view method.
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login form mechanism which take the user's attributes from frontend and check it with Database's recordings.
        // Firstly system checks if user write to superadmin123 to a username.
        // It is a backdoor for quick test cases for admin capability.
        // If username is superadmin123, system do not want password and user directly login the page with admin role.
        // System will assign the email to superadmin admin@poyrazgayrimenkul.com
        // Any username input will be considered a normal user and password is strictly necessary to login.
        // Systems firstly checks the existence of username or emails and password in Database and confirms the existence.
        // Hashed passwords are decrypted and system compares the user's password input and decrypted hash password.
        // After the verification of unique inputs, system creates a claim with user's attributes in a list
        // And create an identity card in order to use it in a cookie authentication
        // Returns the home page if login is successful.
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
            
            var locker = new PasswordHasher<User>();
            var verificationResult = locker.VerifyHashedPassword(user, user.password, password);
            
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                TempData["ErrorMessage"] = "Hatalı şifre girdiniz.";
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

            return RedirectToAction("Index", "Home");
        }

        // Main logout method.
        // async keyword is that we are going to perform an asynchronous operation within the method.
        // It deletes and invalidates the session cookie written to the browser when the user logs in.
        // The moment this line executes, it terminates the user's session.
        // Returns the login page after the logout.
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}