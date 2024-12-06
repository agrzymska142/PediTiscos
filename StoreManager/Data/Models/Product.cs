using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreManager.Data.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation Property
        public Category Category { get; set; }
    }
}
