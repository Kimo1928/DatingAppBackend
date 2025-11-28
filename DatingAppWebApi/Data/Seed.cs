using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using System.Security.Cryptography;

namespace DatingAppWebApi.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DatingAppDbContext context)
        {
            if (context.Users.Any()) return;
            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = System.Text.Json.JsonSerializer.Deserialize<List<SeedUserDTO>>(userData, options);
            if (users == null) return;
            using var hmac=new HMACSHA512(); 

            foreach (var user in users)
            {
                var appUser = new AppUser
                {
                    Id = user.Id,
                    Email = user.Email,
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Pa$$w0rd")),
                    PasswordSalt = hmac.Key,
                    DisplayName = user.DisplayName,
                    ImageUrl = user.ImageUrl,
                    User=new User { 
                        Id = user.Id,
                        DateOfBirth = user.DateOfBirth,
                        City = user.City,
                        Country = user.Country,
                        DisplayName = user.DisplayName,
                        ImageUrl = user.ImageUrl,
                        Description = user.Description,
                        Gender = user.Gender,
                        Created = user.Created,
                        LastActive = user.LastActive

                    }
                   
                };
                appUser.User.Photos.Add(new Photo
                {
                    Url = user.ImageUrl!,
                    UserId = user.Id,
                });
                context.AppUsers.Add(appUser);
            }
            await context.SaveChangesAsync();
        }
    }
}
