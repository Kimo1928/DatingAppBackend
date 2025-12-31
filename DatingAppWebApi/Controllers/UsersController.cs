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
    public class UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService): BaseController
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


        [HttpPost("add-photo")]
        public async Task<IActionResult> uploadPhoto([FromForm]IFormFile file) {
            var user = await userRepository.GetUserForUpdate(User.GetUserId());
            if(user==null) return BadRequest("User not found");
            var result = await photoService.UploadPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                UserId = user.Id
            };
            if (user.ImageUrl==null)
            {
                user.ImageUrl = photo.Url;
                user.AppUser.ImageUrl = photo.Url;
            }
            user.Photos.Add(photo);
            if (await userRepository.SaveAllAsync())
            {
                return Ok(photo);
            }
            return BadRequest("Problem adding photo");
        }


        [HttpPut("set-main-photo/{photoId}")]
        public async Task<IActionResult> changeMainPhoto(int photoId) { 
        var user = await userRepository.GetUserForUpdate(User.GetUserId());
            if (user == null) return BadRequest("User not found");
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null) return BadRequest("Photo not found");
            if(user.ImageUrl==photo.Url) return BadRequest("This is already your main photo");
            user.ImageUrl = photo.Url;
            user.AppUser.ImageUrl = photo.Url;
            if (await userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to set main photo");


        }




        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeleteImage(int photoId) { 
        
        var user = await userRepository.GetUserForUpdate(User.GetUserId());
            if (user == null) return BadRequest("User not found");
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null) return BadRequest("Photo not found");
            if (user.ImageUrl == photo.Url) return BadRequest("You cannot delete your main photo");
            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if (await userRepository.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("Failed to delete photo");

        }




    }
}
