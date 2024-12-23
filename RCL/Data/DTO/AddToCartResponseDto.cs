using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCL.Data.DTO
{
    internal class AddToCartResponseDto
    {
        public string Message { get; set; }
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
