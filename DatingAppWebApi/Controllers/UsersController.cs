using AutoMapper;
using DatingAppWebApi.ActionFilters;
using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Interfaces;
using DatingAppWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Controllers
{
   
     [Authorize]
    public class UsersController(IUserRepository userRepository,Mapper mapper): BaseController
    {
        
        [ServiceFilter (typeof (UserActionFilter))]

        [HttpGet]   
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userRepository.GetAllUsersAsync();
            var usersToReturn=mapper.Map<IReadOnlyList<User>>(users);
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
    }
}
