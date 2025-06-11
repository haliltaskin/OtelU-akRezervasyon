using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OtelUçakRezervasyon.DTOS;
using OtelUçakRezervasyon.Models;
using OtelUçakRezervasyon.Services;

namespace OtelUçakRezervasyon.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _singInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = Guid.NewGuid().ToString() 
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                var subject = "Kayıt Başarılı - Rezervasyon Sistemine Hoş Geldiniz";
                var body = $"<b>Merhaba {model.FullName},</b><br/><br/>" +
                           $"Sistemimize başarıyla kayıt oldunuz.<br/>" +
                           $"Giriş yaparak otel ve uçak rezervasyonlarınızı yönetebilirsiniz.<br/><br/>" +
                           $"İyi günler dileriz.<br/><br/>Rezervasyon Ekibi";

                await _emailService.SendEmailAsync(model.Email, subject, body);

                return StatusCode(201);
            }

            return BadRequest(result.Errors);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest(new { message = "email hatalı" });
            }

            var result = await _singInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                return Ok(
                    new { token = GenerateJWT(user) }
                );
            }
            return Unauthorized();
        }


        private async Task<object> GenerateJWT(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value ?? "");

            // 🔽 Kullanıcının veritabanındaki rolünü al
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Customer"; // Rol yoksa default: Customer

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Role, role) // 🔥 Gerçek rol buraya yazılıyor
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
