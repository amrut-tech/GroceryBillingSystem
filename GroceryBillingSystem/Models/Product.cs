using System.ComponentModel.DataAnnotations;

namespace GroceryBillingSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
