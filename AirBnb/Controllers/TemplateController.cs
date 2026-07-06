using AirBnb.Models.Template;
using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Controllers;

public class TemplateController : Controller
{
    public IActionResult Index()
    {
        var model = new TemplateDashboardViewModel
        {
            Kpis =
            [
                new TemplateKpiItem { Label = "Aktif İlan", Value = "12" },
                new TemplateKpiItem { Label = "Bekleyen Rezervasyon", Value = "4" },
                new TemplateKpiItem { Label = "Aylık Gelir", Value = "₺124.500" },
                new TemplateKpiItem { Label = "Yeni Yorum", Value = "9" }
            ],
            RecentListings =
            [
                new TemplateListingSummary { Title = "Kadıköy Loft", City = "İstanbul", PricePerNight = "₺3.250", Status = "Aktif" },
                new TemplateListingSummary { Title = "Çeşme Yazlık", City = "İzmir", PricePerNight = "₺5.800", Status = "Taslak" },
                new TemplateListingSummary { Title = "Uludağ Chalet", City = "Bursa", PricePerNight = "₺4.900", Status = "Aktif" }
            ],
            RecentReservations =
            [
                new TemplateReservationSummary { GuestName = "Ayşe Y.", ListingTitle = "Kadıköy Loft", DateRange = "12.08 - 15.08", TotalPrice = "₺9.750", Status = "Onaylandı" },
                new TemplateReservationSummary { GuestName = "Mehmet K.", ListingTitle = "Uludağ Chalet", DateRange = "19.08 - 21.08", TotalPrice = "₺9.800", Status = "Beklemede" }
            ]
        };

        return View(model);
    }
}
