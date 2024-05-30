using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.CartDTOs;
using SmartZonePhoneShop.DTO.UserDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        public IConfiguration _configuration;
        public readonly ApplicationDbContext _context;

        

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userData)
        {
            if (userData != null && userData.Username != null && userData.Password != null)
            {
                var user = await GetUserCredentials(userData.Username, userData.Password);

                if (user != null)
                {
                    // Generisanje JWT tokena
                    var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("user_id", user.UserId.ToString()),
                new Claim("username", user.Username),
                new Claim("usertype_id", user.UserTypeId.ToString())
            };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(120),
                        signingCredentials: creds
                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return Unauthorized("Invalid username or password.");
                }
            }

            return BadRequest(ModelState);
        }

        private async Task<UserCheckDTO> GetUserCredentials(string username, string password)
        {
            var user = await _context.Users
                            .Include(u => u.UserType)
                            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                return new UserCheckDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    UserTypeId = user.UserTypeId
                };
            }

            return null;
        }





        //[HttpPost("login")]

        //public async Task<IActionResult> Login(UserLoginDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Provera korisničkog imena i lozinke
        //        if (CheckUserCredentials(model.Username, model.Password))
        //        {
        //            // Generisanje JWT tokena
        //            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
        //            var token = GenerateJwtToken(user);

        //            return Ok(new { token });
        //        }
        //        else
        //        {
        //            return Unauthorized("Invalid username or password.");
        //        }
        //    }

        //    return BadRequest(ModelState);
        //}





        //private bool CheckUserCredentials(string username, string password)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        //    return user != null;
        //}


        //private string GenerateJwtToken(User user)
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        //        new Claim("user_id", user.UserId.ToString()),
        //        new Claim("username", user.Username),
        //        new Claim("usertype_id", user.UserTypeId.ToString())
        //    };

        //    //Dodavanje UserType ako postoji
        //    if (user.UserType != null)
        //    {
        //        claims.Add(new Claim("usertype_id", user.UserType.UserTypeId.ToString()));
        //        if (user.UserType.UserTypeId == 1)
        //        {
        //            claims.Add(new Claim("usertype_name", "Administrator"));
        //        }
        //        else if (user.UserType.UserTypeId == 2)
        //        {
        //            claims.Add(new Claim("usertype_name", "Kupac"));
        //        }
        //    }

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(120),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


    }
}
