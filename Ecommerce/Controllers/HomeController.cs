//HomeController.cs
using System.Diagnostics;
using Ecommerce.Models;
using Ecommerce.Models.Context;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceContext _context;

        public HomeController(ILogger<HomeController> logger, EcommerceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch the 3 categories: Panjabi, T-shirt, Shirt
            var categories = _context.Category
                .Where(c => c.Title == "Panjabi" || c.Title == "Pant/Trouser" || c.Title == "Shirt")
                .OrderBy(c => c.CategoryId)
                .ToList();

            return View(categories); // Send to Index.cshtml
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
}
