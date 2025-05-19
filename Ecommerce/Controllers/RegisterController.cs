//RegisterController
using Ecommerce.Models.ViewModels;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models.Context;

namespace Ecommerce.Controllers
{
    public class RegisterController : Controller
    {
        private readonly EcommerceContext _ecommerceContext;
        public RegisterController(EcommerceContext ecommerceContext)
        {
            _ecommerceContext = ecommerceContext;
        }

        public IActionResult CreateRegister()
        {
            var viewModel = new RegisterViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult CreateRegister(RegisterViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Check if email already exists
            bool emailExists = _ecommerceContext.UserInfo.Any(u => u.Email == viewModel.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(viewModel);
            }
            if (viewModel.Role != "Student" && viewModel.Role != "Teacher" && viewModel.Role != "Admin")
            {
                ModelState.AddModelError("", "Invalid role selected.");
                return View(viewModel);
            }


            // Map ViewModel to User entity
            var user = new UserInfo
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                PasswordHash = new PasswordHasher<UserInfo>().HashPassword(null, viewModel.Password),
                Role = viewModel.Role
            };

            // Save to database
            _ecommerceContext.UserInfo.Add(user);
            _ecommerceContext.SaveChanges();


            // Redirect to login or home page
            return RedirectToAction("CreateLogin", "Login");
        }

    }
}