using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.Json;

namespace DatingAppWebApi.Data
{
    public class Seed
    {
        public static async Task SeedUsers(
       UserManager<AppUser> userManager,
       RoleManager<IdentityRole> roleManager)
        {
            // 1️⃣ Ensure roles exist
            string[] roles = { "Member", "Moderator", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (await userManager.FindByEmailAsync("admin@test.com") == null)
            {
                var admin = new AppUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    DisplayName = "Admin"
                };

                var result = await userManager.CreateAsync(admin, "Pa$$word");

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
                }
            }

            // 3️⃣ Seed normal users ONCE
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<SeedUserDTO>>(userData, options);
            if (users == null) return;

            foreach (var user in users)
            {
                var appUser = new AppUser
                {
                    Email = user.Email,
                    UserName = user.Email,
                    DisplayName = user.DisplayName,
                    ImageUrl = user.ImageUrl,
                    User = new User
                    {
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

                appUser.User.Photos.Add(new Photo { Url = user.ImageUrl! });

                var result = await userManager.CreateAsync(appUser, "Pa$$w0rd");
                if (!result.Succeeded) continue;

                await userManager.AddToRoleAsync(appUser, "Member");
            }
        }

    }
}
