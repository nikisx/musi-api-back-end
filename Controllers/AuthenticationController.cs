using MusicApi.Authentication;
using MusicApi.Common;
using MusicApi.JsonModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext dbContext;
        private readonly AppSettings _appSettings;

        public AuthenticateController(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            _configuration = configuration;
            this.dbContext = dbContext;
            this._appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
               var token = GetEncryptedJWT(user, _appSettings.Secret);

                return Ok(new
                {
                    Token = token,
                    Data = user,
                    Success = true,
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                return BadRequest(new Response<int>
                {
                    Status = "Failed",
                    Success = false,
                    Message = "User with that username already exists!",
                });
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                { UserName = model.Username, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new Response<int>
                    {
                        Status = "Success",
                        Success = true,
                        Message = "User registerd!",
                    });
                }
                else
                {
                    return BadRequest(new Response<int>
                    {
                        Status = "Failed",
                        Success = false,
                        Message = string.Join(", ", result.Errors),
                    });
                }
            }

            return BadRequest();
        }

        [Route("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {

            ApplicationUser applicationUser = await userManager.GetUserAsync(User);

            return Ok(applicationUser);
        }

        private string GetEncryptedJWT(ApplicationUser user, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }

    }
}
