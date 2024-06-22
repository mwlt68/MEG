using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MEG.Demo.ElasticLogger.Api.DataAccess.DbContext;
using MEG.Demo.ElasticLogger.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MEG.Demo.ElasticLogger.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LibraryDbContext _libraryDbContext;

        public AuthController(LibraryDbContext libraryDbContext)
        {
            _libraryDbContext = libraryDbContext;
        }


        [HttpPost]
        public IActionResult PostAsync([FromBody] string username)
        {
            var user = _libraryDbContext.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
            if (user is null)
                return NotFound();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IServiceCollectionExtension.JwtKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                claims: new List<Claim>()
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString())
                },
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return Ok(new { Token = tokenString, UserId = user.Id.ToString() });
        }
    }
}