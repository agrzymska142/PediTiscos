﻿namespace Backend.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? CategoryName { get; set; }

        public bool IsActive { get; set; }

        public string? ImageUrl { get; set; }
    }
}
