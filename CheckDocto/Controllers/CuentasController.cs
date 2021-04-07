using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CheckDocto.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheckDocto.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Obsolete]  // Sólo para ignorar salida a swagger
    public class CuentasController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public CuentasController(
            //UserManager<ApplicationUser> userManager,
            //SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            //_userManager = userManager;
            //_signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Crear")]
        public ActionResult<UserToken> CreateUser([FromBody] UserInfo model)
        {
            //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (result.Succeeded)
            //{
                return BuildToken(model, new List<string>());
            //}
            //else
            //{
            //    return BadRequest("Username or password invalid");
            //}

        }

        /*
        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByEmailAsync(userInfo.Email);
                var roles = await _userManager.GetRolesAsync(usuario);
                return BuildToken(userInfo, roles);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
        }
        */

        private UserToken BuildToken(UserInfo userInfo, IList<string> roles)
        {
            var claims = new List<Claim>
            {
        new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
        new Claim("Codevsys", "Mobile Autoventa-Starfood"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            //foreach (var rol in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, rol));
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddYears(Int32.Parse(_configuration["jwt:ExpirationYears"]));

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
