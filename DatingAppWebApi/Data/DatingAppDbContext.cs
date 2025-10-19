using DatingAppWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Data
{
    public class DatingAppDbContext(DbContextOptions options) : DbContext(options)
    {
        public  DbSet<AppUser> Users { get; set; }

       

    }
}
