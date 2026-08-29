using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using restaurantAPI.DTO.logDto;
using RestaurantReservationSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
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

                var Token = new JwtSecurityToken(
                    claims: userdata,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: sigcer
                    );

                var stringtaken = new JwtSecurityTokenHandler().WriteToken(Token);
                //return Ok(stringtaken);
                Response.Cookies.Append("jwt", stringtaken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
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
