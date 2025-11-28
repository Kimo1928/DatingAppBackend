using DatingAppWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Data
{
    public class DatingAppDbContext(DbContextOptions options) : DbContext(options)
    {
        public  DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }





    }
}
