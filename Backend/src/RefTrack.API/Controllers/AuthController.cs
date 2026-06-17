using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefTrack.Application.Interface;
using RefTrack.Domain.Entities;
using RefTrack.Infrastructure.Persistence;

namespace RefTrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDBContext _db;
        private readonly ITokenService _tokenService;
        public AuthController(AppDBContext db, ITokenService tokenService)
        { _db = db; _tokenService = tokenService; }
        // POST api/auth/register
        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
        {
            var exists = await _db.AppUsers.AnyAsync(u => u.Email == req.Email.ToLower(), ct);
            if (exists) return BadRequest(new { error = "Email already registered." });
            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);
            var user = AppUser.Create(req.Email.ToLower(), req.Email, hash);
            _db.AppUsers.Add(user);
            await _db.SaveChangesAsync(ct);
            var token = _tokenService.GenerateToken(user);
            return Ok(new { token, userId = user.Id, email = user.Email });
        }
        // POST api/auth/login
        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest req, CancellationToken ct)
        {
            var user = await _db.AppUsers
            .FirstOrDefaultAsync(u => u.Email == req.Email.ToLower(), ct);
            if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                return Unauthorized(new { error = "Invalid email or password." });
            var token = _tokenService.GenerateToken(user);
            return Ok(new { token, userId = user.Id, email = user.Email });
        }
    }
}
