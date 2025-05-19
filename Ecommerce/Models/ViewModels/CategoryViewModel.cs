namespace Ecommerce.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Category Category { get; set; } = new Category();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
