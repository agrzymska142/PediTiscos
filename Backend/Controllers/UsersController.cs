using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var user = new ApplicationUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(user);
        }

        // POST: api/users/{id}/roles
        [HttpPost("{id}/roles")]
        public async Task<IActionResult> AddRoleToUser(string id, [FromBody] string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");

            if (!await _roleManager.RoleExistsAsync(role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                    return BadRequest("Failed to create role");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Role {role} added to user {user.UserName}");
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"User {user.UserName} deleted successfully");
        }
    }

    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
