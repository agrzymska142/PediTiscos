﻿namespace Backend.Dtos
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
