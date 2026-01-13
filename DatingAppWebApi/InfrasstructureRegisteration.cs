using DatingAppWebApi.ActionFilters;
using DatingAppWebApi.Data;
using DatingAppWebApi.Helpers;
using DatingAppWebApi.Interfaces;
using DatingAppWebApi.Repositories;
using DatingAppWebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace DatingAppWebApi
{
    public static class InfrasstructureRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddDbContext<DatingAppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<UserActionFilter>();
            services.AddControllers(options =>
            {
                options.Filters.Add(new UserActionFilter());
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<LogUserActivity>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var tokenKey = configuration["TokenKey"] ?? throw new Exception("Cannot get Token Key-program.cs ");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            return services;
        }
    }
}
