using DatingAppWebApi.ActionFilters;
using DatingAppWebApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppWebApi.Controllers
{

    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly DatingAppDbContext _context;
        public BaseController(DatingAppDbContext context)
        {
            _context = context;
        }
        public BaseController()
        {
        }
    }
}
