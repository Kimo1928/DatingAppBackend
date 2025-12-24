using AutoMapper;
using DatingAppWebApi.ActionFilters;
using DatingAppWebApi.Data;
using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Extensions;
using DatingAppWebApi.Interfaces;
using DatingAppWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DatingAppWebApi.Controllers
{
   
     [Authorize]
    public class UsersController(IUserRepository userRepository,IMapper mapper): BaseController
    {
        
        [ServiceFilter (typeof (UserActionFilter))]

        [HttpGet]   
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userRepository.GetAllUsersAsync();
            var usersToReturn=mapper.Map<IReadOnlyList<GetUserDTO>>(users);
            return Ok(usersToReturn);
        }

        /*
         public ActionResult<IReadOnlyList<AppUser>> GetAllUsers() {
        
        var users = _context.Users.AsNoTracking().ToList();
        return users ;
        
        }
         */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id) { 
        
        var user =await userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userToReturn = mapper.Map<User>(user);
            return Ok(userToReturn);
        }
        [HttpGet("{id}/photos")]
        public async Task<IActionResult> GetUserPhotos(string id)
        {
            var photos = await userRepository.GetUserPhotos(id);
            return Ok(photos);
        }

        public async Task<IActionResult> UpdateUser(UserUpdateDTO userUpdateDTO) {
            var userId=User.GetUserId();
            var user = await userRepository.GetUserForUpdate(userId);
            if (user == null) return NotFound();
            user.AppUser.DisplayName = userUpdateDTO.DisplayName ?? user.AppUser.DisplayName;
            user.DisplayName = userUpdateDTO.DisplayName ?? user.DisplayName;
            user.City = userUpdateDTO.City ?? user.City;
            user.Country = userUpdateDTO.Country ?? user.Country;
            user.Description = userUpdateDTO.Description ?? user.Description;
            if(await userRepository.SaveAllAsync())
            return NoContent();
            return BadRequest("Failed to update user");
        }
    }
}
