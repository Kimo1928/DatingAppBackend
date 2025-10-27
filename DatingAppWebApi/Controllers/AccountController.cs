using DatingAppWebApi.Data;
using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Extensions;
using DatingAppWebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppWebApi.Controllers
{
   
    public class AccountController(DatingAppDbContext context,ITokenService tokenService) : BaseController(context)
    {

            [HttpPost("Register")]
            public async Task<IActionResult> Register( [FromBody] RegisterDTO registerDTO)
        {
           
            using  var hmac=new HMACSHA512();
            if (await EmailExists(registerDTO.Email))
            {
                return BadRequest("Email is already taken");
            }
            var user = new AppUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key


            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user.ToDto(tokenService));

        }

        private Task<bool> EmailExists(string email) {

            return _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        
        }


        [HttpPost("login")]

        public async Task<IActionResult> login([FromBody] LoginDTO loginDTO) {

            var user = await   _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginDTO.Email.ToLower());


            if((user == null))
            {
                return Unauthorized("Invalid Email or password");
            }

            using 
             var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Email or password");
                }
            }
            return Ok(user.ToDto(tokenService));

        }
    }
}
