using EisenringDamianLB_295.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using EisenringDamianLB_295.Context;

namespace EisenringDamianLB_295.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;


        public AuthController(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public ActionResult Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return Ok(newUser); // Hier sollte ein OkObjectResult zurückgegeben werden
        }



        [HttpPost("login")]
        public ActionResult Login(UserDto request)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null)
            {
                return BadRequest("Benutzer nicht gefunden.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Falsches Passwort.");
            }

            string token = CreateToken(user);

            return Ok("Logged in Succesfully");
        }

        private string CreateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            

            return jwt;
        }
    }
}
