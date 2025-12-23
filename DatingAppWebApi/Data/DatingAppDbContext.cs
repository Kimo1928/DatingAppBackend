using DatingAppWebApi.Entities;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
namespace DatingAppWebApi.Data
{
    public class DatingAppDbContext(DbContextOptions options) : DbContext(options)
    {
        public  Microsoft.EntityFrameworkCore.DbSet<AppUser> AppUsers { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Photo> Photos { get; set; }





    }
}
