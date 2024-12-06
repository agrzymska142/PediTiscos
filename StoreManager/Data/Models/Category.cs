using System.ComponentModel.DataAnnotations;

namespace StoreManager.Data.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Navigation Property
        public ICollection<Product> Products { get; set; }
    }
}
