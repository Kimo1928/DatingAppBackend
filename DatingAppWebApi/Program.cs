
using DatingAppWebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi
{
    public class Program
    {
        public static void Main(string[] args)
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

                x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }); 
            app.UseAuthentication(); 

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
