using Backend.Dtos;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly PediTiscosDbContext _context;

    public ProductsController(PediTiscosDbContext context)
    {
        _context = context;
    }

    // 1. Visualize all available products
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableProducts()
    {
        var products = await _context.Products
            .Where(p => p.Stock > 0)
            .Include(p => p.Category)
            .ToListAsync();

        var productDtos = products.Select(p => new ProductDto
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = p.Category?.Name
        }).ToList();

        return Ok(productDtos);
    }

    // 2. Filter products by category
    [HttpGet("by-category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategory(int categoryId)
    {
        var products = await _context.Products
            .Where(p => p.CategoryId == categoryId && p.Stock > 0)
            .Include(p => p.Category)
            .ToListAsync();

        var productDtos = products.Select(p => new ProductDto
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryName = p.Category?.Name
        }).ToList();

        return Ok(productDtos);
    }

    // 3. View a randomly featured product
    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedProduct()
    {
        var product = await _context.Products
            .Where(p => p.Stock > 0)
            .OrderBy(r => Guid.NewGuid())
            .FirstOrDefaultAsync();

        if (product == null)
            return NotFound("No featured product found.");

        var productDto = new ProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryName = product.Category?.Name
        };

        return Ok(productDto);
    }
}
