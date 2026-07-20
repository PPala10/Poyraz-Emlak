using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirbnbClone.Data;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Payment = AirBnb.Data.Payment;

namespace AirBnb.Controllers
{
    // Controller for Payment Entity's Page with MVC Protocol
    public class PaymentController : Controller
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly Options _iyzicoOptions;

        // I used iyzico's infrastructure to take payment and handle it in sandbox test environment.
        // API and SecretKey keep in appsettings.json file and I took it from iyzico's developer website.
        public PaymentController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            _iyzicoOptions = new Options
            {
                ApiKey = _configuration["Iyzico:ApiKey"],
                SecretKey = _configuration["Iyzico:SecretKey"],
                BaseUrl = _configuration["Iyzico:BaseUrl"]
            };
        }
    
        // Main user page (index) view method.
        // This pages shows all reservations which their payments completed and their iyzico's number 
        [HttpGet]
        public IActionResult Index()
        {
            var payments = _context.Payments
                .Include(p => p.reservation)
                    .ThenInclude(r => r.listing)
                .Include(p => p.reservation)
                    .ThenInclude(r => r.guest)
                .OrderByDescending(p => p.created_at)
                .ToList();

            return View(payments);
        }

        // Checkout mechanism which receives the reservation ID from the request and initializes the iyzico payment process.
        // The method first fetches the reservation from the database along with its associated listing and guest details.
        // If the reservation exists, it constructs the payment request including buyer, shipping, billing, and basket item details.
        // After setting up the request payload, the method calls the iyzico API to generate the checkout form.
        // If initialization is successful, it passes the form content to the View;
        // otherwise, it redirects to the Reservation page with an error.
        [HttpGet]
        public async Task<IActionResult> CheckOut(int reservationId)
        {
            var reservation = _context.Reservations
                .Include(r => r.listing)
                .Include(r => r.guest)
                .FirstOrDefault(r => r.reservationId == reservationId);

            if (reservation == null)
            {
                TempData["ErrorMessage"] = "Ödeme yapılacak rezervasyon bulunamadı!";
                return RedirectToAction("Index", "Reservation");
            }

            var request = new CreateCheckoutFormInitializeRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = reservation.reservationId.ToString(),
                Price = reservation.total_price.ToString("F2").Replace(",", "."),
                PaidPrice = reservation.total_price.ToString("F2").Replace(",", "."),
                Currency = Currency.TRY.ToString(),
                BasketId = "B" + reservation.reservationId,
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                CallbackUrl = $"{Request.Scheme}://{Request.Host}/Payment/Callback?reservationId={reservation.reservationId}"
            };

            var buyer = new Buyer
            {
                Id = reservation.guestId.ToString(),
                Name = reservation.guest?.fname ?? "Bilinmeyen",
                Surname = reservation.guest?.lname ?? "Misafir",
                GsmNumber = "+905555555555",
                Email = reservation.guest?.email ?? "info@poyrazgayrimenkul.com",
                IdentityNumber = "11111111111",
                RegistrationAddress = "Barbaros Mah. Karanfil Sok. No:12",
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34746"
            };
            request.Buyer = buyer;

            var shippingAddress = new Address
            {
                ContactName = $"{reservation.guest?.fname} {reservation.guest?.lname}",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Barbaros Mah. Karanfil Sok. No:12",
                ZipCode = "34746"
            };
            request.ShippingAddress = shippingAddress;
            request.BillingAddress = shippingAddress;

            var basketItems = new List<BasketItem>();
            var firstBasketItem = new BasketItem
            {
                Id = reservation.listId.ToString(),
                Name = reservation.listing?.title ?? "Konaklama Hizmeti",
                Category1 = "Konaklama",
                ItemType = BasketItemType.VIRTUAL.ToString(),
                Price = reservation.total_price.ToString("F2").Replace(",", ".")
            };
            basketItems.Add(firstBasketItem);
            request.BasketItems = basketItems;

            var checkoutFormInitialize = await CheckoutFormInitialize.Create(request, _iyzicoOptions);

            if (checkoutFormInitialize.Status == "success")
            {
                ViewBag.CheckoutFormContent = checkoutFormInitialize.CheckoutFormContent;
                return View(reservation);
            }

            TempData["ErrorMessage"] = "iyzico ödeme formu başlatılamadı: " + checkoutFormInitialize.ErrorMessage;
            return RedirectToAction("Index", "Reservation");
        }

        // Callback mechanism which receives the payment token from iyzico and the reservation ID from the URL.
        // The method validates the incoming parameters and queries the iyzico API to retrieve the payment status.
        // Upon verifying that the payment was successful, it updates the reservation status to 'Confirmed'.
        // It then creates and persists a new Payment record in the database with the transaction details.
        // Redirects to the Reservation page with a success message on completion, or an error message if verification fails.
        [HttpPost]
        public async Task<IActionResult> Callback([FromForm] string token, [FromQuery] int reservationId)
        {
            try
            {
                if (reservationId <= 0)
                {
                    TempData["ErrorMessage"] = "Doğrulama Hatası: URL parametresinden geçerli bir Rezervasyon ID alınamadı.";
                    return RedirectToAction("Index", "Reservation");
                }

                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Doğrulama Hatası: iyzico token bilgisi boş döndü.";
                    return RedirectToAction("Index", "Reservation");
                }

                var request = new RetrieveCheckoutFormRequest
                {
                    Locale = Locale.TR.ToString(),
                    Token = token
                };

                var checkoutForm = await CheckoutForm.Retrieve(request, _iyzicoOptions);

                if (checkoutForm == null)
                {
                    TempData["ErrorMessage"] = "Doğrulama Hatası: İyzico sunucularından yanıt alınamadı.";
                    return RedirectToAction("Index", "Reservation");
                }

                if (checkoutForm.Status != "success")
                {
                    TempData["ErrorMessage"] = $"iyzico Hatası: {checkoutForm.ErrorMessage} (Kod: {checkoutForm.ErrorCode})";
                    return RedirectToAction("Index", "Reservation");
                }

                var reservation = _context.Reservations.FirstOrDefault(r => r.reservationId == reservationId);
                if (reservation == null)
                {
                    TempData["ErrorMessage"] = $"Veritabanı Hatası: #{reservationId} numaralı rezervasyon kaydı sistemde bulunamadı.";
                    return RedirectToAction("Index", "Reservation");
                }

                if (checkoutForm.PaymentStatus == "SUCCESS")
                {
                    reservation.status = "Confirmed";

                    var payment = new Payment
                    {
                        reservationId = reservation.reservationId,
                        amount = reservation.total_price,
                        currency = "TRY",
                        status = "Success",
                        provider = "iyzico",
                        provider_tx_id = checkoutForm.PaymentId,
                        provider_response = checkoutForm.Status,
                        paid_at = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                        created_at = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                    };

                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Ödemeniz iyzico güvencesiyle başarıyla alındı ve rezervasyonunuz onaylandı!";
                    return RedirectToAction("Index", "Reservation");
                }
                else
                {
                    TempData["ErrorMessage"] = $"Banka Onay Hatası: Ödeme banka tarafından reddedildi. Durum: {checkoutForm.PaymentStatus}";
                    return RedirectToAction("Index", "Reservation");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Sistem Kritik Hatası: {ex.Message}";
                return RedirectToAction("Index", "Reservation");
            }
        }
    }
}