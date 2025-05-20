//ProductController.cs
using Ecommerce.Models;
using Ecommerce.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly EcommerceContext _context;

        public ProductController(EcommerceContext context)
        {
            _context = context;
        }

        public IActionResult IndexProduct(int categoryId)
        {
            var products = _context.Product
                                   .Where(p => p.CategoryId == categoryId)
                                   .Include(p => p.Category)
                                   .ToList();

            ViewBag.CategoryId = categoryId;
            return View(products);
        }

        public IActionResult CreateProduct(int categoryId)
        {
            var product = new Product { CategoryId = categoryId };
            return View(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Product.Add(product);
                _context.SaveChanges();
                return RedirectToAction("IndexProduct", new { categoryId = product.CategoryId });
            }

            return View(product);
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.Product.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Product.Update(product);
                _context.SaveChanges();
                return RedirectToAction("IndexProduct", new { categoryId = product.CategoryId });
            }
            return View(product);
        }
    }
}
