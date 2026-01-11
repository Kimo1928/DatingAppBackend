using DatingAppWebApi.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.ActionFilters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var resultContext = await next();
            if (context.HttpContext.User.Identity?.IsAuthenticated !=true) { return; }

            var userId = resultContext.HttpContext.User.GetUserId();

            var dbContext = resultContext.HttpContext.RequestServices
                .GetService<DatingAppWebApi.Data.DatingAppDbContext>();
            if (dbContext == null) { return; }
            await dbContext.Users.Where(u => u.Id == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x=>x.LastActive , DateTime.UtcNow));

        }
    }
}
