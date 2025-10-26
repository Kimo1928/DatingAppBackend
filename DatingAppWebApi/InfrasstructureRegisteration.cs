using DatingAppWebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi
{
    public  static class InfrasstructureRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services ,IConfiguration configuration) {

            services.AddControllers();
            services.AddDbContext<DatingAppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddCors();
            return services;
        }
    }
}
