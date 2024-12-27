using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.Data.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]  // You can adjust the length depending on your requirements
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number")]
        public int Stock { get; set; } = 0;  // Default value of 0

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation Property
        public Category Category { get; set; }

        public bool IsActive { get; set; } = true;

        public string? ImageUrl { get; set; }
    }
}
