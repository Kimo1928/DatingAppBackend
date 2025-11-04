using DatingAppWebApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppWebApi.Controllers
{
   
    public class BuggyController() : BaseController()
    {
        [HttpGet("auth")]
        public IActionResult GetAuth() {

            return Unauthorized();
        }
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound("Not Found");
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("this is a server error");
        }
        [HttpGet("bad-request")]
        public IActionResult GetBadrequest() { 
        
        return BadRequest("This was not a good request");
        }

    }
}
