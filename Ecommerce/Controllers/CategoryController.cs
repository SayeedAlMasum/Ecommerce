//CourseController.cs
using Ecommerce.Models;
using Ecommerce.Models.Context;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Ecommerce.Controllers
{
    public class CategoryController : Controller
    {
        private readonly EcommerceContext _ecommerceContext;

        public CategoryController(EcommerceContext ecommerceContext)
        {
            _ecommerceContext = ecommerceContext;
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        public IActionResult IndexCategory()
        {
            var viewModel = new CategoryViewModel
            {
                Categories = _ecommerceContext.Category
                            .OrderBy(c => c.CategoryId)
                            .ToList()
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory()
        {
            var viewModel = new CategoryViewModel
            {
                Categories = _ecommerceContext.Category
                            .OrderBy(c => c.CategoryId)
                            .ToList(),
                Category = new Category()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _ecommerceContext.Category.Add(viewModel.Category);
                _ecommerceContext.SaveChanges();
                return RedirectToAction("CreateCategory");
            }

            viewModel.Categories = _ecommerceContext.Category
                                  .OrderBy(c => c.CategoryId)
                                  .ToList();
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult EditCategory(int id)
        {
            // Fetch the course from the database using the course id
            var category = _ecommerceContext.Category.FirstOrDefault(c => c.CategoryId == id);

            // If the course does not exist, return a 404 error page
            if (category == null)
            {
                return NotFound();
            }

            // Return the Category to the view for editing
            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditCategory(Category updatedCategory)
        {
            if (ModelState.IsValid)
            {
                // Get the existing Category by ID
                var existing = _ecommerceContext.Category.FirstOrDefault(c => c.CategoryId == updatedCategory.CategoryId);
                if (existing == null) return NotFound();

                // Update values
                existing.Title = updatedCategory.Title;

                _ecommerceContext.SaveChanges();

                return RedirectToAction("CreateCategory");
            }

            return View(updatedCategory);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            // Find Category by ID
            var category = _ecommerceContext.Category.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            // Remove Category
            _ecommerceContext.Category.Remove(category);
            _ecommerceContext.SaveChanges();

            return RedirectToAction("CreateCategory");
        }

    }
}