using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers;

public class HomeController(ILogger<HomeController> logger, OnlineShopContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var banners = await context.Banners.ToListAsync();
        if (banners == null) logger.LogError("No banners");
        ViewData["banners"] = banners;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
