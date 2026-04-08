
using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;
using DatingAppWebApi.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi
{
    public class Program
    {
        public  static async  Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Services part in my program.cs

           
            InfrasstructureRegisteration.AddInfrastructureServices(builder.Services,builder.Configuration);

            //builder.Services.AddControllers();
            //builder.Services.AddDbContext<DatingAppDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            //Middlewares Part in my program.cs

            app.UseMiddleware<Middleware.ExceptionMiddleware>();
            app.UseCors(x => {

                x.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200");
            }); 
            app.UseAuthentication(); 

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<PresenceHub>("hubs/presence");
            app.MapHub<MessageHub>("hubs/messages");
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<DatingAppDbContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await context.Database.MigrateAsync();

                    await context.Connections.ExecuteDeleteAsync();
                    
                    await Seed.SeedUsers(userManager,roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }
            app.Run();
        }
    }
}
