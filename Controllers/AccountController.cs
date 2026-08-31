using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using restaurantAPI.DTO.logDto;
using restaurantAPI.models;
using restaurantAPI.Models.Context;
using RestaurantReservationSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly RestaurantDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            RestaurantDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
            {   
                var user =await _userManager.FindByNameAsync(dto.UserName);
                if (user == null) { return Unauthorized(); }

                var password = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (!password) {  return Unauthorized(); }

               var roles = await _userManager.GetRolesAsync(user);

                List<Claim> userdata = new List<Claim>();
                userdata.Add(new Claim(ClaimTypes.Name,dto.UserName ));
                userdata.Add(new Claim ( ClaimTypes.MobilePhone, "01111111111"));

                foreach (var role in roles) {
                    userdata.Add(new Claim(ClaimTypes.Role, role));
                };

                string secertkey = _configuration["Jwt:Key"];
                var key =new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secertkey));

                var sigcer =new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(15);

            var Token = new JwtSecurityToken(
                claims: userdata,
                expires: accessTokenExpiration,
                signingCredentials: sigcer
            );

            var stringtoken =
                new JwtSecurityTokenHandler().WriteToken(Token);

            // 6. Create Refresh Token
            var refreshToken = Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64)
            );

            // 7. Save Refresh Token in database
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();

            // 8. Put Access Token in cookie
            Response.Cookies.Append("jwt", stringtoken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = new DateTimeOffset(accessTokenExpiration)
            });

            // 9. Put Refresh Token in cookie
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(new
                {
                    message = "Login successful"
                });

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                return BadRequest("Role does not exist.");
            }

            var user = new ApplicationUser
            {
                UserName = dto.UserName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok("User registered successfully.");
        }
    }
}
