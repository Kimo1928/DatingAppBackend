using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Controllers
{
   
    public class UsersController : BaseController
    {
        public UsersController(DatingAppDbContext context):base(context){ 
        
        
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }

        /*
         public ActionResult<IReadOnlyList<AppUser>> GetAllUsers() {
        
        var users = _context.Users.AsNoTracking().ToList();
        return users ;
        
        }
         */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id) { 
        
        var user =await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
