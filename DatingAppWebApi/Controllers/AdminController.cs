using DatingAppWebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Controllers
{
    public class AdminController(UserManager<AppUser> userManager) : BaseController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles() {
            var users= await userManager.Users.ToListAsync();
            var userList = new List<object>();
            foreach (var user in users) {
            var roles= await userManager.GetRolesAsync(user);
                userList.Add(new {
                user.Id,
                user.Email,
                Roles = roles.ToList()
                });
            
            }
            return Ok(userList);
        }



        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-role/{userId}")]
        public async Task<ActionResult> EditRole(string userId, [FromQuery] string roles) {

            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles=roles.Split(',').ToArray();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest("Could not retrieve user");
            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));


            if (!result.Succeeded) return BadRequest("Can't add new roles to user");

            result = await userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles));
            if (!result.Succeeded) return BadRequest("Can't remove old roles from user");

            return Ok(await userManager.GetRolesAsync(user));
        
        }

        [Authorize(Policy = "ModeratePhotoRole")]

        [HttpGet("photo-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {

            return Ok("Admin pr moderators can see this ");
        }
    }
}
