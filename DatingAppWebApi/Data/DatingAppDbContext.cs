using DatingAppWebApi.Entities;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace DatingAppWebApi.Data
{
    public class DatingAppDbContext(DbContextOptions options) : DbContext(options)
    {
        public  Microsoft.EntityFrameworkCore.DbSet<AppUser> AppUsers { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<Photo> Photos { get; set; }


        public DbSet<Message> Messages { get; set; }

        public DbSet<UserLike> Likes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(t => t.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(t => t.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);


            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
             v => v.HasValue? v.Value.ToUniversalTime():null,
             v =>v.HasValue? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
             );
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) )
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }


    }
}
