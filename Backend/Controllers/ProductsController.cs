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

    // 1. GET /api/Products: Retrieve a list of all products with filtering and searching
    [HttpGet]
    public async Task<IActionResult> GetAllProducts(
        [FromQuery] int? categoryId,
        [FromQuery] string? search,
        [FromQuery] bool? available)
    {
        var query = _context.Products.AsQueryable();

        // Filter by category
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        // Search by name or description
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));

        // Filter by availability
        if (available.HasValue)
            query = query.Where(p => available.Value ? p.Stock > 0 : p.Stock == 0);

        var products = await query
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

    // 2. GET /api/Products/{id}: Retrieve a specific product by its ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product == null)
            return NotFound($"Product with ID {id} not found.");

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

    // 3. GET /api/Products/Featured: Retrieve a featured product
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


    [HttpPost("AddToCart")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
    {
        // Validate input
        if (addToCartDto.Quantity <= 0)
            return BadRequest("Quantity must be greater than 0.");

        var product = await _context.Products.FindAsync(addToCartDto.ProductId);
        if (product == null)
            return NotFound($"Product with ID {addToCartDto.ProductId} not found.");

        if (product.Stock < addToCartDto.Quantity)
            return BadRequest("Not enough stock available.");

        // Simulate adding to cart (for now, reduce stock directly)
        product.Stock -= addToCartDto.Quantity;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Product added to cart.",
            RemainingStock = product.Stock
        });
    }
}
