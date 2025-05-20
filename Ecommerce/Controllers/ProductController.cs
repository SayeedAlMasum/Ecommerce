//ProductController.cs
using Ecommerce.Models;
using Ecommerce.Models.Context;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult IndexProduct(int? categoryId)
        {
            var productsQuery = _context.Product.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
                ViewBag.CategoryId = categoryId.Value;
            }
            else
            {
                ViewBag.CategoryId = null;
            }

            var products = productsQuery.ToList();
            return View(products);
        }


        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult CreateProduct(int categoryId)
        {
            var model = new ProductViewModel
            {
                CategoryId = categoryId,
                Products = _context.Product
                                   .Where(p => p.CategoryId == categoryId)
                                   .ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    CategoryId = model.CategoryId
                };

                _context.Product.Add(newProduct); // FIXED HERE
                _context.SaveChanges();

                return RedirectToAction("CreateProduct", new { categoryId = model.CategoryId }); // PASS categoryId
            }

            // If model is invalid, return the same view with validation errors
            model.Products = _context.Product
                                     .Where(p => p.CategoryId == model.CategoryId)
                                     .ToList();
            return View(model);
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

        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Product.Find(id);
            if (product == null)
                return NotFound();
            _context.Product.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("IndexProduct", new { categoryId = product.CategoryId });
        }
    }
}
