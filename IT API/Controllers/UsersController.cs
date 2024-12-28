using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ProductContext _context;

        public UsersController(ProductContext context)
        {
            _context = context;
        }
        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {
            if (newUser == null)
                return BadRequest("Invalid user data.");

            // Przykład sprawdzenia czy email już jest w bazie:
            var emailExists = await _context.Users.AnyAsync(u => u.Email == newUser.Email);
            if (emailExists)
                return BadRequest("Email is already used by another account.");

            // Zapis do bazy (UWAGA: w produkcji hasła haszujemy!)
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest("Invalid login data.");

            // Znajdź użytkownika w bazie po nazwie użytkownika
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.Username == request.Username);

            // Jeśli nie znaleziono lub hasło nieprawidłowe -> błąd
            if (user == null)
                return Unauthorized("Invalid username or password.");

            // Zakładając, że hasło w bazie nie jest hashowane (na potrzeby przykładu):
            // W rzeczywistości:
            // if(!BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) return Unauthorized(...);
            if (user.Password != request.Password)
                return Unauthorized("Invalid username or password.");

            // Jeśli dotarliśmy tutaj, dane logowania są prawidłowe
            return Ok("Login successful.");
        }

        // Inne metody (Register itd.)

    }

    // Klasa pomocnicza do przechwycenia danych logowania
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
