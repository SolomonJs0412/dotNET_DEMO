using System.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetEFAndJWT.classes;
using dotnetEFAndJWT.JWTHelpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace dotnetEFAndJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContext _dbContext;

        public AuthController(IConfiguration configuration, DataContext dbContext)
        {
            _config = configuration;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            if (users.Count == 0) return NotFound();
            return Ok(users);
        }

        public static User user = new User();
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> Register([FromBody] UserDto req)
        {
            CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var name = req.Username;
            var user = new User(name, passwordHash, passwordSalt);
            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Register), new { Username = req.Username }, user);
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserDto req)
        {
            var userDb = _dbContext.Users.SingleOrDefault(u => u.Username == req.Username);
            if (userDb == null)
            {
                return NotFound("Not found");
            }

            if (!VerifyPassword(req.Password, userDb.PasswordHash, userDb.PasswordSalt))
            {
                return BadRequest("Invalid password");
            }

            string token = CreatedToken(user);
            return Ok(token);
        }

        private string CreatedToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("Key:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(23),
                signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA1())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA1(passwordSalt))
            {
                var cmpHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return cmpHash.SequenceEqual(passwordHash);
            }
        }
    }
}