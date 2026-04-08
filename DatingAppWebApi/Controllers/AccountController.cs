using DatingAppWebApi.Data;
using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Extensions;
using DatingAppWebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppWebApi.Controllers
{
   
    public class AccountController(UserManager<AppUser> userManager,ITokenService tokenService) : BaseController()
    {

            [HttpPost("Register")]
            public async Task<IActionResult> Register( [FromBody] RegisterDTO registerDTO)
        {
           
           
            var user = new AppUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.UserName,
                UserName = registerDTO.Email,
                User =new User {
                                 DisplayName=registerDTO.UserName,
                                 Country=registerDTO.Country,
                                 Gender=registerDTO.Gender,
                                 City=registerDTO.City ,
                                 DateOfBirth = registerDTO.DateOfBirth
                }


            };
            var result = await userManager.CreateAsync(user,registerDTO.Password);
            if (!result.Succeeded)
            { foreach (var error in result.Errors)
                    ModelState.AddModelError("identity",error.Description);
                return ValidationProblem();
                
            }
            await userManager.AddToRoleAsync(user, "Member");
            await SetRefreshTokenCookie(user);
            return Ok(await user.ToDto(tokenService));

        }

      


        [HttpPost("login")]

        public async Task<IActionResult> login([FromBody] LoginDTO loginDTO) {

            var Appuser = await userManager.FindByEmailAsync(loginDTO.Email);
            


            if((Appuser == null))
            {
                return Unauthorized("Invalid Email or password");
            }
            var result = await userManager.CheckPasswordAsync(Appuser,loginDTO.Password);
            if (!result) { 
            return Unauthorized("Invalid Email or Password");
            }
            await SetRefreshTokenCookie(Appuser);


            return Ok(await Appuser.ToDto(tokenService));

        }


        private async Task SetRefreshTokenCookie(AppUser user) { 
        var refreshToken = tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,                    
                SameSite = SameSiteMode.None,      
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/"

            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDTO>> RefreshToken() {

            var refreshToken = Request.Cookies["refreshToken"];
            if(refreshToken==null) return NoContent();
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken &&
            u.RefreshTokenExpiryTime > DateTime.UtcNow
            );
            if (user==null) return Unauthorized();
            await SetRefreshTokenCookie(user);
            return await user.ToDto(tokenService);
        } 


    }
}
