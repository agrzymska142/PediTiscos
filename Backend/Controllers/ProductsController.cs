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

    //Pobierz listę wszystkich produktów z filtrowaniem i wyszukiwaniem
    [HttpGet]
    public async Task<IActionResult> GetAllProducts(
        [FromQuery] int? categoryId,
        [FromQuery] string? search,
        [FromQuery] bool? available)
    {
        var query = _context.Products.AsQueryable();

        // Filtrowanie według kategorii
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId);

        // Wyszukiwanie według nazwy lub opisu
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));

        // Filtrowanie według dostępności
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
            CategoryName = p.Category?.Name,
            IsActive = p.IsActive,
            ImageUrl = p.ImageUrl
        }).ToList();

        return Ok(productDtos);
    }

    //Pobierz konkretny produkt według jego ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        // Pobieranie produktu wraz z kategorią
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
            CategoryName = product.Category?.Name,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl
        };

        return Ok(productDto);
    }

    // Pobierz wyróżniony produkt
    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedProduct()
    {
        // Pobieranie losowego produktu, który jest dostępny w magazynie
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
        
        if (addToCartDto.Quantity <= 0)
            return BadRequest("Quantity must be greater than 0.");

        // Pobieranie produktu z bazy danych
        var product = await _context.Products.FindAsync(addToCartDto.ProductId);
        if (product == null)
            return NotFound($"Product with ID {addToCartDto.ProductId} not found.");

        if (product.Stock < addToCartDto.Quantity)
            return BadRequest("Not enough stock available.");

        // Pobieranie identyfikatora użytkownika
        var userIdentifier = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : Request.Headers["sessionId"].ToString();

        // Sprawdź, czy produkt jest już w koszyku
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserIdentifier == userIdentifier && ci.ProductId == addToCartDto.ProductId);

        if (cartItem == null)
        {
            // Dodaj nowy produkt do koszyka
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
            // Zaktualizuj ilość
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

    // Pobierz produkty z koszyka
    [HttpGet("cart")]
    public async Task<IActionResult> GetCartItems()
    {
        // Pobierz identyfikator użytkownika
        var userIdentifier = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : Request.Headers["sessionId"].ToString();

        // Pobieranie produktów z koszyka wraz z produktami
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

    // Zaktualizuj produkt w koszyku
    [HttpPut("UpdateCartItem")]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
    {
        // Pobierz identyfikator użytkownika
        var userIdentifier = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : Request.Headers["sessionId"].ToString();

        // Pobieranie produktu z koszyka
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserIdentifier == userIdentifier && ci.ProductId == updateCartItemDto.ProductId);

        if (cartItem == null)
            return NotFound($"Cart item with Product ID {updateCartItemDto.ProductId} not found.");

        if (updateCartItemDto.Quantity <= 0)
            return BadRequest("Quantity must be greater than 0.");

        // Zaktualizuj ilość
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

    // Usuń produkt z koszyka
    [HttpDelete("RemoveFromCart/{productId}")]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        // Pobierz identyfikator użytkownika
        var userIdentifier = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : Request.Headers["sessionId"].ToString();

        // Pobieranie produktu z koszyka
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserIdentifier == userIdentifier && ci.ProductId == productId);

        if (cartItem == null)
            return NotFound($"Cart item with Product ID {productId} not found.");

        // Usuń produkt z koszyka
        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Cart item removed.",
            CartItemId = cartItem.CartItemId,
            ProductId = cartItem.ProductId
        });
    }

    // Złóż zamówienie
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

        
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Order placed successfully.", OrderId = order.OrderId });
    }

    // Pobierz historię zamówień
    [HttpGet("order-history")]
    public async Task<IActionResult> GetOrderHistory()
    {
        // Sprawdź, czy użytkownik jest zalogowany
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized("User must be logged in to view order history.");
        }

        // Pobierz identyfikator użytkownika
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Pobierz zamówienia użytkownika wraz ze szczegółami zamówień i produktami
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
