//PaymentController.cs
using Ecommerce.Models.Context;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Ecommerce.Controllers
{
    public class PaymentController : Controller // Inherit from Controller
    {
        private readonly EcommerceContext _ecommerceContext;

        public PaymentController(EcommerceContext ecommerceContext)
        {
            _ecommerceContext = ecommerceContext;
        }

        // Displays the payment form for a given course

        public IActionResult CreatePayment(int courseId)
        {
            // Retrieve the course from the database by ID
            var category = _ecommerceContext.Category.FirstOrDefault(c => c.CategoryId == courseId);
            if (category == null)
            {
                return NotFound();// Return 404 if course doesn't exist
            }

            // Preparing the view model to show course title on the form
            var viewModel = new PaymentViewModel
            {
                Category = category
            };

            return View("CreatePayment", viewModel);// Show payment form
        }

        // Process after submitting the payment form
        [HttpPost]
        public IActionResult CreatePayment(PaymentViewModel viewModel)
        {
            // If model state is invalid (e.g., missing or invalid input), redisplay the form with errors
            if (!ModelState.IsValid)
            {
                if (viewModel.Category?.CategoryId != null)
                {
                    viewModel.Category = _ecommerceContext.Category
                        .FirstOrDefault(c => c.CategoryId == viewModel.Category.CategoryId);
                }
                return View("CreatePayment", viewModel);
            }

            // Simulate successful payment (no real payment gateway here)
            TempData["PaymentSuccess"] = "Payment successful for the course: " + viewModel.Category?.Title;

            // Redirect back to course list page
            return RedirectToAction("IndexCourse", "Course");
        }
    }
}