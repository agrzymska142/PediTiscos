using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        // Get user identifier
        var userIdentifier = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : Request.Headers["sessionId"].ToString();

        // Check if the product is already in the cart
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserIdentifier == userIdentifier && ci.ProductId == addToCartDto.ProductId);

        if (cartItem == null)
        {
            // Add new item to the cart
            cartItem = new CartItem
            {
                UserIdentifier = userIdentifier,
                ProductId = addToCartDto.ProductId,
                Quantity = addToCartDto.Quantity,
                Product = product
            };
            _context.CartItems.Add(cartItem);
        }
        else
        {
            // Update quantity
            cartItem.Quantity += addToCartDto.Quantity;
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Product added to cart.",
            CartItemId = cartItem.CartItemId,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity
        });
    }

    [HttpGet("cart")]
    public async Task<IActionResult> GetCartItems()
    {
        var userIdentifier = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : Request.Headers["sessionId"].ToString();

        var cartItems = await _context.CartItems
            .Where(ci => ci.UserIdentifier == userIdentifier)
            .Include(ci => ci.Product)
            .ToListAsync();

        var cartItemDtos = cartItems.Select(ci => new CartItemDto
        {
            CartItemId = ci.CartItemId,
            ProductId = ci.ProductId,
            Name = ci.Product.Name,
            Description = ci.Product.Description,
            Price = ci.Product.Price,
            Quantity = ci.Quantity
        }).ToList();

        return Ok(cartItemDtos);
    }

    [HttpPut("UpdateCartItem")]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
    {
        var userIdentifier = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : Request.Headers["sessionId"].ToString();

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserIdentifier == userIdentifier && ci.ProductId == updateCartItemDto.ProductId);

        if (cartItem == null)
            return NotFound($"Cart item with Product ID {updateCartItemDto.ProductId} not found.");

        if (updateCartItemDto.Quantity <= 0)
            return BadRequest("Quantity must be greater than 0.");

        cartItem.Quantity = updateCartItemDto.Quantity;
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Cart item updated.",
            CartItemId = cartItem.CartItemId,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity
        });
    }

    [HttpDelete("RemoveFromCart/{productId}")]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userIdentifier = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : Request.Headers["sessionId"].ToString();

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserIdentifier == userIdentifier && ci.ProductId == productId);

        if (cartItem == null)
            return NotFound($"Cart item with Product ID {productId} not found.");

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Cart item removed.",
            CartItemId = cartItem.CartItemId,
            ProductId = cartItem.ProductId
        });
    }

    [HttpPost("PlaceOrder")]
    public async Task<IActionResult> PlaceOrder()
    {
        // Sprawdź, czy użytkownik jest zalogowany
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized("User must be logged in to place an order.");
        }

        // Pobierz identyfikator użytkownika
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst("name")?.Value;
        var userSurname = User.FindFirst("surname")?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            return BadRequest("User email is required to place an order.");
        }

        // Pobierz produkty z koszyka użytkownika
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserIdentifier == userId)
            .Include(ci => ci.Product)
            .ToListAsync();

        if (!cartItems.Any())
        {
            return BadRequest("Cart is empty.");
        }

        // Oblicz całkowitą kwotę zamówienia
        var totalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

        // Utwórz nowe zamówienie
        var order = new Order
        {
            OrderDate = DateTime.Now,
            ClientName = $"{userName} {userSurname}",
            ClientEmail = userEmail,
            TotalAmount = totalAmount,
            Status = "Pending",
            OrderDetails = cartItems.Select(ci => new OrderDetail
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList()
        };

        _context.Orders.Add(order);

        // Zaktualizuj stan magazynowy produktów
        foreach (var cartItem in cartItems)
        {
            var product = cartItem.Product;
            product.Stock -= cartItem.Quantity;
            if (product.Stock < 0)
            {
                return BadRequest($"Not enough stock for product {product.Name}.");
            }
        }

        // Usuń produkty z koszyka
        _context.CartItems.RemoveRange(cartItems);

        // Zapisz zmiany w bazie danych
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Order placed successfully.", OrderId = order.OrderId });
    }

    [HttpGet("order-history")]
    public async Task<IActionResult> GetOrderHistory()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized("User must be logged in to view order history.");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var orders = await _context.Orders
            .Where(o => o.ClientEmail == userId)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ToListAsync();

        var orderDtos = orders.Select(o => new OrderDto
        {
            UserId = userId,
            OrderDate = o.OrderDate,
            ClientName = o.ClientName,
            ClientEmail = o.ClientEmail,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
            {
                ProductId = od.ProductId,
                ProductName = od.Product.Name,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            }).ToList()
        }).ToList();

        return Ok(orderDtos);
    }

}
