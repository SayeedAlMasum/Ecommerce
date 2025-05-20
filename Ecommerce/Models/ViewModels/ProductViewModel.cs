using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecommerce.Models.ViewModels
{
    public class ProductViewModel
    {
        public int CategoryId { get; set; }

        // Fields for creating a new product
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        // Existing products to display
        [ValidateNever]
        public List<Product> Products { get; set; }
    }
}
