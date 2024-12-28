using Backend.Dtos;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly PediTiscosDbContext _context;

    public CategoriesController(PediTiscosDbContext context)
    {
        _context = context;
    }

    // Pobieranie wszystkich kategorii
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Pobieranie listy kategorii z bazy danych
        var categories = await _context.Categories.ToListAsync();
        // Mapowanie kategorii na DTO
        var categoryDtos = categories.Select(c => new CategoryDto
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description
        }).ToList();

        return Ok(categoryDtos);
    }
}
