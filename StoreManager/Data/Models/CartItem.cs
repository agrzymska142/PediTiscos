using System.ComponentModel.DataAnnotations;

namespace StoreManager.Data.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public string UserIdentifier { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
