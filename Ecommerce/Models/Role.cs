//Role.cs
using System.ComponentModel.DataAnnotations;
using Ecommerce.Models;

namespace Ecommerce.Models
{
    public class Role : BaseModel
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}