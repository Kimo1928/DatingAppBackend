using DatingAppWebApi.Data;
using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DatingAppWebApi.Controllers
{
   
    public class AccountController(DatingAppDbContext context) : BaseController(context)
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
            return Ok(user);

        }

        private Task<bool> EmailExists(string email) {

            return _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        
        }


    }
}
